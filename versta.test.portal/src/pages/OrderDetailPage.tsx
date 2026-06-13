import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchOrderById, deleteOrder } from '../api/orders';
import type { OrderDto } from '../types/order';

const OrderDetailPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [order, setOrder] = useState<OrderDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [deleting, setDeleting] = useState(false);

    useEffect(() => {
        if (!id) return;
        const load = async () => {
            try {
                const data = await fetchOrderById(id);
                setOrder(data);
            } catch (err) {
                setError('Заказ не найден');
            } finally {
                setLoading(false);
            }
        };
        load();
    }, [id]);

    const handleDelete = async () => {
        if (!id || !window.confirm('Вы уверены, что хотите удалить этот заказ?')) return;
        setDeleting(true);
        try {
            await deleteOrder(id);
            navigate('/orders');
        } catch (err) {
            setError('Ошибка при удалении');
        } finally {
            setDeleting(false);
        }
    };

    if (loading) return <div>Загрузка...</div>;
    if (error && !order) return <div style={{ color: 'red' }}>{error}</div>;
    if (!order) return null;

    return (
        <div>
            <h2>Заказ №{order.id}</h2>
            <p><strong>Дата создания:</strong> {new Date(order.createdAt).toLocaleString()}</p>
            <p><strong>Город отправителя:</strong> {order.fromAddress.city}</p>
            <p><strong>Адрес отправителя:</strong> {order.fromAddress.address}</p>
            <p><strong>Город получателя:</strong> {order.toAddress.city}</p>
            <p><strong>Адрес получателя:</strong> {order.toAddress.address}</p>
            <p><strong>Вес груза:</strong> {order.weightKg} кг</p>
            <p><strong>Дата забора груза:</strong> {new Date(order.deliveryDate).toLocaleDateString()}</p>

            <div style={{ marginTop: '20px' }}>
                <button onClick={() => navigate(`/orders/${id}/edit`)}>Редактировать</button>
                <button onClick={handleDelete} disabled={deleting} style={{ marginLeft: '10px' }}>
                    {deleting ? 'Удаление...' : 'Удалить'}
                </button>
                <button onClick={() => navigate('/orders')} style={{ marginLeft: '10px' }}>← Назад к списку</button>
            </div>
            {error && <div style={{ color: 'red', marginTop: '10px' }}>{error}</div>}
        </div>
    );
};

export default OrderDetailPage;