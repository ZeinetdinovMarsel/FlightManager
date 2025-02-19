// src/api/flight.ts

import axiosInstance from "../utils/axiosInstance";
import { API_URL } from "../utils/axiosInstance";

interface Flight {
    id: number;
    flightNumber: string;
    destination: string;
    departureTime: string;
    arrivalTime: string;
    availableSeats: number;
    airplanePhotoUrl: string;
    airportId: number;
}

export const getFlights = async (
    sortBy?: string,
    descending?: boolean,
    page?: number,
    pageSize?: number,
    flightNumberFilter?: string,
    destinationFilter?: string,
    departureTimeFilter?: string,
    arrivalTimeFilter?: string,
    availableSeatsFilter?: number,
    airportIdFilter?: number
) => {
    const response = await axiosInstance.get(`${API_URL}flights`, {
        params: {
            sortBy,
            descending,
            page,
            pageSize,
            flightNumberFilter,
            destinationFilter,
            departureTimeFilter,
            arrivalTimeFilter,
            availableSeatsFilter,
            airportIdFilter
        },
    });
    return response.data;
};

export const getFlightById = async (id: number) => {
    const response = await axiosInstance.get(`${API_URL}flights/${id}`);
    return response.data;
};

export const createFlight = async (
    flightNumber: string,
    destination: string,
    departureTime: string,
    arrivalTime: string,
    availableSeats: number,
    airplanePhotoUrl: string,
    airportId: number
) => {
    const flight: Flight = {
        id: 0,
        flightNumber,
        destination,
        departureTime,
        arrivalTime,
        availableSeats,
        airplanePhotoUrl,
        airportId
    };
    const response = await axiosInstance.post(`${API_URL}flights`, flight);
    return response.data;
};

export const updateFlight = async (
    id: number,
    flightNumber: string,
    destination: string,
    departureTime: string,
    arrivalTime: string,
    availableSeats: number,
    airplanePhotoUrl: string,
    airportId: number
) => {
    const flight: Flight = {
        id,
        flightNumber,
        destination,
        departureTime,
        arrivalTime,
        availableSeats,
        airplanePhotoUrl,
        airportId
    };
    const response = await axiosInstance.put(`${API_URL}flights/${id}`, flight);
    return response.data;
};

export const deleteFlight = async (id: number) => {
    const response = await axiosInstance.delete(`${API_URL}flights/${id}`);
    return response.data;
};