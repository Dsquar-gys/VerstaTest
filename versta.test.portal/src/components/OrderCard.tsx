import React from 'react';
import type {OrderDto} from '../types/order';

interface OrderCardProps {
    order: OrderDto;
    onClick: () => void;
}

const OrderCard: React.FC<OrderCardProps> = ({order, onClick}) => {
    return (
        <div className="order-card" onClick={onClick}
             style={{cursor: 'pointer', border: '1px solid #ccc', margin: '8px', padding: '8px'}}>
            <div><strong>Номер заказа:</strong> {order.id}</div>
            <div><strong>Отправитель:</strong> {order.fromAddress.city}, {order.fromAddress.address}</div>
            <div><strong>Получатель:</strong> {order.toAddress.city}, {order.toAddress.address}</div>
            <div><strong>Вес:</strong> {order.weightKg} кг</div>
            <div><strong>Дата забора:</strong> {new Date(order.deliveryDate).toLocaleDateString()}</div>
        </div>
    );
};

export default OrderCard;