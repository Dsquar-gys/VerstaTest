export interface DeliveryAddress {
    city: string;
    address: string;
}

export interface OrderDto {
    id: string;
    createdAt: string;
    fromAddress: DeliveryAddress;
    toAddress: DeliveryAddress;
    weightKg: number;
    deliveryDate: string;
}

export interface CreateOrderRequest {
    fromAddress: DeliveryAddress;
    toAddress: DeliveryAddress;
    weightKg: number;
    deliveryDate: string;
}

export interface PatchOrderRequest {
    fromAddress?: DeliveryAddress;
    toAddress?: DeliveryAddress;
    weightKg?: number;
    deliveryDate?: string;
}

export interface GetPageRequest {
    page?: number;
    pageSize?: number;
}

export interface GetPageResponse<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
    totalPages: number;
}