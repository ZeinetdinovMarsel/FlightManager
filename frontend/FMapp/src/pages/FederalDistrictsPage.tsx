// src/pages/FederalDistrictPage.tsx
import React from 'react';
import { getFederalDistricts, deleteFederalDistrict, updateFederalDistrict, createFederalDistrict } from '../api/federalDistrict';
import DataTable from '../components/DataTable';

const FederalDistrictPage: React.FC = () => {

    const columns = [
        { id: 'id', label: 'id', sortable: true },
        { id: 'name', label: 'federalDistrictName', sortable: true },
    ];

    const fetchData = async (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => {
        const data = await getFederalDistricts(sortBy, descending, page, pageSize, filters.name);
        return data;
    };

    const addItem = async (item: any) => {
        const federalDistrict = await createFederalDistrict(item.name);
        return federalDistrict;
    };

    const updateItem = async (item: any) => {
        await updateFederalDistrict(item.id, item.name);
        return item;
    };

    const deleteItem = async (id: number) => {
        await deleteFederalDistrict(id);
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
            }}
            initialSortBy="id"
            initialDescending={false}
        />
    );
};

export default FederalDistrictPage;