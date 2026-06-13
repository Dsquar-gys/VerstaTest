import React from 'react';
import './App.css';
import {BrowserRouter, Link, Route, Routes} from 'react-router-dom';
import CreateOrderPage from './pages/CreateOrderPage';
import OrderListPage from './pages/OrderListPage';
import OrderDetailPage from './pages/OrderDetailPage';
import EditOrderPage from "./pages/EditOrderPage";

const App: React.FC = () => {
    return (
        <BrowserRouter>
            <nav>
                <Link to="/orders/new">Создать заказ</Link> |{' '}
                <Link to="/orders">Список заказов</Link>
            </nav>
            <Routes>
                <Route path="/orders/new" element={<CreateOrderPage/>}/>
                <Route path="/orders/:id/edit" element={<EditOrderPage />} />
                <Route path="/orders/:id" element={<OrderDetailPage/>}/>
                <Route path="/orders" element={<OrderListPage/>}/>
                <Route path="/" element={<OrderListPage/>}/>
            </Routes>
        </BrowserRouter>
    );
};

export default App;