import React, { useEffect, useState } from 'react';
import { getTickets, deleteTicket, updateTicket, createTicket, TicketRequest } from '../api/ticket';
import DataTable from '../components/DataTable';
import { getServices } from '../api/service';
import { Service } from '../api/service'; // Предполагается, что у вас есть интерфейс Service

const TicketPage: React.FC = () => {
    const [services, setServices] = useState<Service[]>([]);

    // Определение столбцов таблицы
    const columns = [
        { id: 'id', label: 'ID', sortable: true },
        { id: 'ticketType', label: 'Тип билета', sortable: true },
        { id: 'price', label: 'Цена', sortable: true },
        { id: 'seat', label: 'Место', sortable: true },
        { id: 'flightId', label: 'ID рейса', sortable: true },
        {
            id: 'services',
            label: 'Услуги',
            sortable: true,
            render: (services: any[]) => {
                if (Array.isArray(services)) {
                    return services.map((service: any) => service.serviceId).join(', ');
                }
                return '';
            },
            type: 'text',
        },
    ];

    // Функция для получения услуг
    const fetchServices = async () => {
        try {
            const data = await getServices();
            setServices(data);
        } catch (error) {
            console.error('Ошибка при получении услуг:', error);
        }
    };

    // Использование useEffect для получения услуг при монтировании компонента
    useEffect(() => {
        fetchServices();
    }, []);

    // Функция для получения данных билетов
    const fetchData = async (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => {
        try {
            const data = await getTickets(
                sortBy,
                descending,
                page,
                pageSize,
                filters.ticketType,
                filters.price,
                filters.seat,
                filters.flightId,
                filters.services,
            );
            return data;
        } catch (error) {
            console.error('Ошибка при получении билетов:', error);
            return { tickets: [], total: 0 };
        }
    };

    // Функция для добавления нового билета
    const addItem = async (item: any) => {
        try {
            const ticket: TicketRequest = {
                ticketType: parseInt(item.ticketType,10),
                price: item.price,
                seat: item.seat,
                flightId: item.flightId,
                services: item.services.map((serviceId: number) => ({ serviceId })),
            };
            console.log(ticket);
            const newTicket = await createTicket(ticket);
            return newTicket;
        } catch (error) {
            console.error('Ошибка при добавлении билета:', error);
            throw error;
        }
    };

    // Функция для обновления существующего билета
    const updateItem = async (item: any) => {
        try {
            const ticket: TicketRequest = {
                ticketType: parseInt(item.ticketType,10),
                price: item.price,
                seat: item.seat,
                flightId: item.flightId,
                services: item.services.map((serviceId: number) => ({ serviceId })),
            };
            const updatedTicket = await updateTicket(item.id, ticket);
            return updatedTicket;
        } catch (error) {
            console.error('Ошибка при обновлении билета:', error);
            throw error;
        }
    };

    // Функция для удаления билета
    const deleteItem = async (id: number) => {
        try {
            await deleteTicket(id);
        } catch (error) {
            console.error('Ошибка при удалении билета:', error);
            throw error;
        }
    };

    return (
        <DataTable
            columns={columns}
            fetchData={fetchData}
            addItem={addItem}
            updateItem={updateItem}
            deleteItem={deleteItem}
            filters={{
                ticketType: null,
                price: null,
                seat: '',
                flightId: null,
                services:[],
            }}
            initialSortBy="id"
            initialDescending={false}
            services={services}
        />
    );
};

export default TicketPage;