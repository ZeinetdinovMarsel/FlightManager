// src/api/service.ts
import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

export interface Service {
    id: number;
    name: string;
    cost: number;
}

export const getServices = async (
    sortBy?: string,
    descending?: boolean,
    page?: number,
    pageSize?: number,
    nameFilter?: string,
    costFilter?: number
) => {
    const response = await axiosInstance.get(`${API_URL}services`, {
        params: { sortBy, descending, page, pageSize, nameFilter, costFilter },
    });
    return response.data;
};

export const getServiceById = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}services/${id}`);
    return response.data;
};

export const createService = async (name: string, cost: number) => {
    const service: Service = {id:0, name, cost };
    const response = await axiosInstance.post(`${API_URL}services`, service);
    return response.data;
};

export const updateService = async (id: number, name: string, cost: number) => {
    const service: Service = { id, name, cost };
    const response = await axiosInstance.put(`${API_URL}services/${id}`, service);
    return response.data;
};

export const deleteService = async (id: number) => {
    const response = await axiosInstance.delete(`${API_URL}services/${id}`);
    return response.data;
};