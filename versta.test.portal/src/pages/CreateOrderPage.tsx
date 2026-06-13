import React, {useState} from 'react';
import {useNavigate} from 'react-router-dom';
import OrderForm from '../components/OrderForm';
import {createOrder} from '../api/orders';
import type {CreateOrderRequest} from '../types/order';

const CreateOrderPage: React.FC = () => {
    const navigate = useNavigate();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (data: CreateOrderRequest) => {
        setIsLoading(true);
        setError(null);
        try {
            await createOrder(data);
            navigate('/orders');
        } catch (err) {
            setError('Ошибка при создании заказа');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div>
            {error && <div style={{color: 'red'}}>{error}</div>}
            <OrderForm onSubmit={handleSubmit} isLoading={isLoading}/>
        </div>
    );
};

export default CreateOrderPage;