import React, { useState, useEffect } from 'react';
import type { OrderDto, PatchOrderRequest } from '../types/order';

interface EditOrderFormProps {
    order: OrderDto;
    onSubmit: (id: string, data: PatchOrderRequest) => Promise<void>;
    isLoading: boolean;
}

const EditOrderForm: React.FC<EditOrderFormProps> = ({ order, onSubmit, isLoading }) => {
    const [fromCity, setFromCity] = useState(order.fromAddress.city);
    const [fromAddress, setFromAddress] = useState(order.fromAddress.address);
    const [toCity, setToCity] = useState(order.toAddress.city);
    const [toAddress, setToAddress] = useState(order.toAddress.address);
    const [weightKg, setWeight] = useState<number>(order.weightKg);
    const [deliveryDate, setDeliveryDate] = useState(order.deliveryDate.split('T')[0]);

    useEffect(() => {
        setFromCity(order.fromAddress.city);
        setFromAddress(order.fromAddress.address);
        setToCity(order.toAddress.city);
        setToAddress(order.toAddress.address);
        setWeight(order.weightKg);
        setDeliveryDate(order.deliveryDate.split('T')[0]);
    }, [order]);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const patchData: PatchOrderRequest = {};
        if (fromCity !== order.fromAddress.city || fromAddress !== order.fromAddress.address) {
            patchData.fromAddress = { city: fromCity, address: fromAddress };
        }
        if (toCity !== order.toAddress.city || toAddress !== order.toAddress.address) {
            patchData.toAddress = { city: toCity, address: toAddress };
        }
        if (weightKg !== order.weightKg) {
            patchData.weightKg = weightKg;
        }
        const newDeliveryDate = new Date(deliveryDate).toISOString();
        if (newDeliveryDate !== order.deliveryDate) {
            patchData.deliveryDate = newDeliveryDate;
        }
        await onSubmit(order.id, patchData);
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Редактирование заказа №{order.id}</h2>
            <div>
                <label>Город отправителя</label>
                <input required value={fromCity} onChange={e => setFromCity(e.target.value)} />
            </div>
            <div>
                <label>Адрес отправителя</label>
                <input required value={fromAddress} onChange={e => setFromAddress(e.target.value)} />
            </div>
            <div>
                <label>Город получателя</label>
                <input required value={toCity} onChange={e => setToCity(e.target.value)} />
            </div>
            <div>
                <label>Адрес получателя</label>
                <input required value={toAddress} onChange={e => setToAddress(e.target.value)} />
            </div>
            <div>
                <label>Вес груза (кг)</label>
                <input type="number" required value={weightKg} onChange={e => setWeight(parseFloat(e.target.value))} />
            </div>
            <div>
                <label>Дата забора груза</label>
                <input type="date" required value={deliveryDate} onChange={e => setDeliveryDate(e.target.value)} />
            </div>
            <button type="submit" disabled={isLoading}>
                {isLoading ? 'Сохранение...' : 'Сохранить изменения'}
            </button>
        </form>
    );
};

export default EditOrderForm;