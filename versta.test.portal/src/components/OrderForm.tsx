import React, {useState} from 'react';
import type {CreateOrderRequest} from '../types/order';

interface OrderFormProps {
    onSubmit: (data: CreateOrderRequest) => Promise<void>;
    isLoading: boolean;
}

const OrderForm: React.FC<OrderFormProps> = ({onSubmit, isLoading}) => {
    const [fromCity, setFromCity] = useState('');
    const [fromAddress, setFromAddress] = useState('');
    const [toCity, setToCity] = useState('');
    const [toAddress, setToAddress] = useState('');
    const [weightKg, setWeight] = useState<number>(0);
    const [deliveryDate, setDeliveryDate] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const request: CreateOrderRequest = {
            fromAddress: {city: fromCity, address: fromAddress},
            toAddress: {city: toCity, address: toAddress},
            weightKg,
            deliveryDate: new Date(deliveryDate).toISOString(),
        };
        await onSubmit(request);
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Новый заказ</h2>
            <div>
                <label>Город отправителя</label>
                <input required value={fromCity} onChange={e => setFromCity(e.target.value)}/>
            </div>
            <div>
                <label>Адрес отправителя</label>
                <input required value={fromAddress} onChange={e => setFromAddress(e.target.value)}/>
            </div>
            <div>
                <label>Город получателя</label>
                <input required value={toCity} onChange={e => setToCity(e.target.value)}/>
            </div>
            <div>
                <label>Адрес получателя</label>
                <input required value={toAddress} onChange={e => setToAddress(e.target.value)}/>
            </div>
            <div>
                <label>Вес груза (кг)</label>
                <input type="number" required value={weightKg} onChange={e => setWeight(parseFloat(e.target.value))}/>
            </div>
            <div>
                <label>Дата забора груза</label>
                <input type="date" required value={deliveryDate} onChange={e => setDeliveryDate(e.target.value)}/>
            </div>
            <button type="submit" disabled={isLoading}>
                {isLoading ? 'Создание...' : 'Создать заказ'}
            </button>
        </form>
    );
};

export default OrderForm;