import React, {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';
import OrderCard from '../components/OrderCard';
import {fetchOrders} from '../api/orders';
import type {OrderDto} from '../types/order';

const OrderListPage: React.FC = () => {
    const navigate = useNavigate();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const pageSize = 20;

    const loadPage = async (pageNumber: number) => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetchOrders({page: pageNumber, pageSize});
            setOrders(response.items);
            setTotalCount(response.totalCount);
            setTotalPages(response.totalPages > 0 ? response.totalPages : 1);
            setPage(response.page);
        } catch (err) {
            setError('Не удалось загрузить заказы');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadPage(1);
    }, []);

    const handlePageChange = (newPage: number) => {
        if (newPage >= 1 && newPage <= totalPages) {
            loadPage(newPage);
        }
    };

    if (loading) return <div>Загрузка...</div>;
    if (error) return <div style={{color: 'red'}}>{error}</div>;

    return (
        <div>
            <h2>Список заказов</h2>
            {orders.length === 0 ? (
                <p>Заказов пока нет</p>
            ) : (
                orders.map(order => (
                    <OrderCard
                        key={order.id}
                        order={order}
                        onClick={() => navigate(`/orders/${order.id}`)}
                    />
                ))
            )}
            {totalCount > 0 && (
                <div style={{marginTop: '20px'}}>
                    <button onClick={() => handlePageChange(page - 1)} disabled={page <= 1}>
                        Предыдущая
                    </button>
                    <span style={{margin: '0 10px'}}>
            Страница {page} из {totalPages}
          </span>
                    <button onClick={() => handlePageChange(page + 1)} disabled={page >= totalPages}>
                        Следующая
                    </button>
                </div>
            )}
        </div>
    );
};

export default OrderListPage;