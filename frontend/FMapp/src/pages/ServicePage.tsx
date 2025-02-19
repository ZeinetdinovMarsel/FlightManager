// src/pages/ServicePage.tsx
import React, { useEffect, useState } from 'react';
import { getServices, deleteService, updateService, createService } from '../api/service';
import DataTableService from '../components/DataTable';

const ServicePage: React.FC = () => {

    // Определение колонок для таблицы услуг
    const columns = [
        { id: 'id', label: 'ID', sortable: true },
        { id: 'name', label: 'Название Услуги', sortable: true },
        { id: 'cost', label: 'Стоимость', sortable: true, type: 'number' },
    ];


    const fetchData = async (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => {
        try {
            const data = await getServices(sortBy, descending, page, pageSize, filters.name, filters.cost);
            return data;
        } catch (error) {
            console.error('Ошибка при получении услуг:', error);
            throw error;
        }
    };

    // Функция для добавления новой услуги
    const addItem = async (item: any) => {
        try {
            const service = await createService(item.name, item.cost);
            return service;
        } catch (error) {
            console.error('Ошибка при добавлении услуги:', error);
            throw error;
        }
    };

    // Функция для обновления существующей услуги
    const updateItem = async (item: any) => {
        try {
            const service = await updateService(item.id, item.name, item.cost);
            return service;
        } catch (error) {
            console.error('Ошибка при обновлении услуги:', error);
            throw error;
        }
    };

    // Функция для удаления услуги
    const deleteItem = async (id: number) => {
        try {
            await deleteService(id);
        } catch (error) {
            console.error('Ошибка при удалении услуги:', error);
            throw error;
        }
    };

    return (
            <DataTableService
                columns={columns}
                fetchData={fetchData}
                addItem={addItem}
                updateItem={updateItem}
                deleteItem={deleteItem}
                filters={{
                    name: '',
                    cost: null,
                }}
                initialSortBy="id"
                initialDescending={false}
            />
    );
};

export default ServicePage;