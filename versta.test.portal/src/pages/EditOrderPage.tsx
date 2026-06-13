import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import EditOrderForm from '../components/EditOrderForm';
import { fetchOrderById, patchOrder } from '../api/orders';
import type { OrderDto, PatchOrderRequest } from '../types/order';

const EditOrderPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [order, setOrder] = useState<OrderDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

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

    const handleSubmit = async (orderId: string, data: PatchOrderRequest) => {
        setSaving(true);
        setError(null);
        try {
            await patchOrder(orderId, data);
            navigate(`/orders/${orderId}`);
        } catch (err) {
            setError('Ошибка при сохранении');
        } finally {
            setSaving(false);
        }
    };

    if (loading) return <div>Загрузка...</div>;
    if (error && !order) return <div style={{ color: 'red' }}>{error}</div>;
    if (!order) return null;

    return (
        <div>
            {error && <div style={{ color: 'red' }}>{error}</div>}
            <EditOrderForm order={order} onSubmit={handleSubmit} isLoading={saving} />
        </div>
    );
};

export default EditOrderPage;