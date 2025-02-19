// src/pages/FlightsPage.tsx
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { getFlights, deleteFlight, updateFlight, createFlight } from '../api/flight';
import DataTable from '../components/DataTable';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { getAirports } from '../api/airport';

const FlightsPage: React.FC = () => {
    const [airports, setAirports] = useState<{ id: number; name: string }[]>([]);
    const columns = [
        { id: 'id', label: 'id', sortable: true },
        { id: 'flightNumber', label: 'flightNumber', sortable: true },
        { id: 'destination', label: 'destination', sortable: true },
        { id: 'departureTime', label: 'departureTime', sortable: true, type: 'date' },
        { id: 'arrivalTime', label: 'arrivalTime', sortable: true, type: 'date' },
        { id: 'availableSeats', label: 'availableSeats', sortable: true, type: 'number' },
        { id: 'airplanePhotoUrl', label: 'airplanePhotoUrl', sortable: true },
        { id: 'airportId', label: 'airportId', sortable: true },
    ];

    const fetchAirports = async () => {
        const data = await getAirports();
        setAirports(data);
    };

    useEffect(() => {
        fetchAirports();
    }, []);

    const fetchData = async (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => {
        const data = await getFlights(sortBy, descending, page, pageSize, filters.flightNumber, filters.destination, filters.departureTime, filters.arrivalTime, filters.availableSeats, filters.airportId);
        return data;
    };

    const addItem = async (item: any) => {
        const departureTime = formatDateTimeForServer(item.departureTime);
        const arrivalTime = formatDateTimeForServer(item.arrivalTime);
        const flight = await createFlight(item.flightNumber, item.destination, departureTime, arrivalTime, item.availableSeats, item.airplanePhotoUrl, item.airportId);
        return flight;
    };

    const updateItem = async (item: any) => {
        const departureTime = formatDateTimeForServer(item.departureTime);
        const arrivalTime = formatDateTimeForServer(item.arrivalTime);
        await updateFlight(item.id, item.flightNumber, item.destination, departureTime, arrivalTime, item.availableSeats, item.airplanePhotoUrl, item.airportId);
        return item;
    };

    const deleteItem = async (id: number) => {
        await deleteFlight(id);
    };

    const formatDateTimeForServer = (dateTime: Date | null): string | null => {
        if (!dateTime) return null;
        return dateTime.toISOString();
    };

    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <DataTable
                columns={columns}
                fetchData={fetchData}
                addItem={addItem}
                updateItem={updateItem}
                deleteItem={deleteItem}
                filters={{
                    flightNumber: '',
                    destination: '',
                    departureTime: null,
                    arrivalTime: null,
                    availableSeats: null,
                    airplanePhotoUrl: '',
                    airportId: null,
                }}
                initialSortBy="id"
                initialDescending={false}
                airports={airports}
            />
        </LocalizationProvider>
    );
};

export default FlightsPage;