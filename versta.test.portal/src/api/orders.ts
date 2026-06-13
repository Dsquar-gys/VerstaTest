import axios from 'axios';
import type {CreateOrderRequest, GetPageRequest, GetPageResponse, OrderDto, PatchOrderRequest} from '../types/order';

const api = axios.create({
    baseURL: '/api',
    headers: {'Content-Type': 'application/json'},
});

export async function fetchOrders(params: GetPageRequest = {}): Promise<GetPageResponse<OrderDto>> {
    const response = await api.get<GetPageResponse<OrderDto>>('/orders', {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 20,
        },
    });
    return response.data;
}

export async function fetchOrderById(id: string): Promise<OrderDto> {
    const response = await api.get<OrderDto>(`/orders/${id}`);
    return response.data;
}

export async function createOrder(data: CreateOrderRequest): Promise<OrderDto> {
    const response = await api.post<OrderDto>('/order', data);
    return response.data;
}

export async function patchOrder(id: string, data: PatchOrderRequest): Promise<OrderDto> {
    const response = await api.patch<OrderDto>(`/orders/${id}`, data);
    return response.data;
}

export async function deleteOrder(id: string): Promise<void> {
    await api.delete(`/orders/${id}`);
}