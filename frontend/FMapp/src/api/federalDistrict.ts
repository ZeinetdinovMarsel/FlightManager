// src/api/federalDistrict.ts
import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

export interface FederalDistrict {
    Id: number;
    Name: string;
}

export const getFederalDistricts = async (
    sortBy?: string,
    descending?: boolean,
    page?: number,
    pageSize?: number,
    nameFilter?: string
) => {
    const response = await axiosInstance.get(`${API_URL}federalDistricts`, {
        params: { sortBy, descending, page, pageSize, nameFilter },
    });
    return response.data;
};

export const getFederalDistrictById = async (id: number) => {
    try {
        const response = await axiosInstance.get(`${API_URL}federalDistricts/${id}`);
        return response.data;
    } catch (error) {
        return null;
    }
};


export const createFederalDistrict = async (name: string) => {
    const federalDistrict: FederalDistrict = { Id: 0, Name: name };
    const response = await axiosInstance.post(`${API_URL}federalDistricts`, federalDistrict);
    console.log(response);
    return response.data;
};

export const updateFederalDistrict = async (id: number, name: string) => {
    const federalDistrict: FederalDistrict = { Id: id, Name: name };
    const response = await axiosInstance.put(`${API_URL}federalDistricts/${id}`, federalDistrict);
    return response.data;
};

export const deleteFederalDistrict = async (id: number): Promise<boolean> => {
    try {
        const response = await axiosInstance.delete(`${API_URL}federalDistricts/${id}`);
        return response.status === 200;
    } catch (error) {
        console.error("Error deleting federal district:", error);
        return false;
    }
};