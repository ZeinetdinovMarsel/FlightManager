import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

export interface Ticket {
    id: number;
    ticketType: number;
    price: number;
    seat: string;
    flightId: number;
    services: TicketService[];
}

export interface TicketRequest {
    ticketType: number;
    price: number;
    seat: string;
    flightId: number;
    services: TicketServiceRequest[];
}

export interface TicketServiceRequest {
    serviceId: number;
}

export interface TicketService {
    ticketId: number;
    serviceId: number;
}

export const getTickets = async (
    sortBy?: string,
    descending?: boolean,
    page?: number,
    pageSize?: number,
    ticketTypeFilter?: number,
    priceFilter?: number,
    seatFilter?: string,
    flightIdFilter?: number,
    serviceIdsFilter?: number[],
) => {
    const response = await axiosInstance.get(`${API_URL}tickets`, {
        params: { sortBy, descending, page, pageSize, ticketTypeFilter, priceFilter, seatFilter, flightIdFilter,serviceIdsFilter },
    });
    return response.data;
};

export const getTicketById = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}tickets/${id}`);
    return response.data;
};

export const createTicket = async (ticket: TicketRequest) => {
    const response = await axiosInstance.post(`${API_URL}tickets`, ticket);
    return response.data;
};

export const updateTicket = async (id: number, ticket: TicketRequest) => {

    const response = await axiosInstance.put(`${API_URL}tickets/${id}`, ticket);
    return response.data;
};

export const deleteTicket = async (id: number) => {
    const response = await axiosInstance.delete(`${API_URL}tickets/${id}`);
    return response.data;
};