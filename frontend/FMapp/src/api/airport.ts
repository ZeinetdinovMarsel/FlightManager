import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

interface Airport {
    name: string;
    city: string;
    federalDistrictId: number;
}

export const getAirports = async (sortBy?: string, descending?: boolean, page?: number, pageSize?: number, cityFilter?: string, nameFilter?: string, federalDistrictIdFilter?: number) => {
    const response = await axiosInstance.get(`${API_URL}airports`, {
        params: { sortBy, descending, page, pageSize, cityFilter, nameFilter, federalDistrictIdFilter },
    });
    return response.data;
};

export const getAirportById = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}airports/${id}`);
    return response.data;
};

export const createAirport = async (name: string, city: string, federalDistrictId: number) => {
    const airport: Airport = { name, city, federalDistrictId };
    const response = await axiosInstance.post(`${API_URL}airports`, airport);
    return response.data;
};

export const updateAirport = async (id: number, name: string, city: string, federalDistrictId: number) => {
    const airport: Airport = { name, city, federalDistrictId };
    const response = await axiosInstance.put(`${API_URL}airports/${id}`, airport);
    return response.data;
};

export const deleteAirport = async (id: number) => {
    const response = await axiosInstance.delete(`${API_URL}airports/${id}`);
    return response.data;
};