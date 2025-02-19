// src/pages/AirportPage.tsx
import React, { useEffect, useState } from 'react';

import { getAirports, deleteAirport, updateAirport, createAirport } from '../api/airport';
import DataTable from '../components/DataTable';
import { getFederalDistricts } from '../api/federalDistrict';

const AirportPage: React.FC = () => {

    const [federalDistricts, setFederalDistricts] = useState<{ id: number; name: string }[]>([]);

    const columns = [
        { id: 'id', label: 'id', sortable: true },
        { id: 'name', label: 'airports', sortable: true },
        { id: 'city', label: 'city', sortable: true },
        { id: 'federalDistrictId', label: 'federalDistrictId', sortable: true },
    ];

    const fetchAirports = async () => {
        const data = await getFederalDistricts();
        setFederalDistricts(data);
    };

    useEffect(() => {
        fetchAirports();
    }, []);

    const fetchData = async (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => {
        const data = await getAirports(sortBy, descending, page, pageSize, filters.city, filters.name, filters.federalDistrictId);
        return data;
    };

    const addItem = async (item: any) => {
        const airport = await createAirport(item.name, item.city, item.federalDistrictId);
        return airport;
    };

    const updateItem = async (item: any) => {
        await updateAirport(item.id, item.name, item.city, item.federalDistrictId);
        return item;
    };

    const deleteItem = async (id: number) => {
        await deleteAirport(id);
    };

    return (
        <DataTable
            columns={columns}
            fetchData={fetchData}
            addItem={addItem}
            updateItem={updateItem}
            deleteItem={deleteItem}
            filters={{
                name: '',
                city: '',
                federalDistrictId: null,
            }}
            initialSortBy="id"
            initialDescending={false}
            federalDistricts={federalDistricts}
        />
    );
};

export default AirportPage;