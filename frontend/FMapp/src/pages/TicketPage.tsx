import React, { useEffect, useState } from 'react';
import { getTickets, deleteTicket, updateTicket, createTicket, TicketRequest, TicketType, getTicketTypes } from '../api/ticket';
import DataTable from '../components/DataTable';
import { getServices } from '../api/service';
import { Service } from '../api/service';
import { Flight, getFlights } from '../api/flight';

const TicketPage: React.FC = () => {
    const [services, setServices] = useState<Service[]>([]);
    const [flights, setFlights] = useState<Flight[]>([]);
    const [ticketTypes, setTicketTypes] = useState<TicketType[]>();

    const columns = [
        { id: 'id', label: 'id', sortable: true },
        { id: 'ticketType', label: 'ticketType', sortable: true, type: 'select' },
        { id: 'price', label: 'price', sortable: true, type: 'number' },
        { id: 'seat', label: 'seat', type: 'number', sortable: true },
        { id: 'flightId', label: 'flightNumber', sortable: true, type: 'select' },
        {
            id: 'services',
            label: 'services',
            sortable: true,
            render: (services: any[]) => {
                if (Array.isArray(services)) {
                    return services.map((service: any) => service.serviceId).join(', ');
                }
                return '';
            },
            type: 'multiselect',
        },
    ];

    const fetchServices = async () => {
        try {
            const data = await getServices();
            setServices(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchFlights = async () => {
        try {
            const data = await getFlights();
            setFlights(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchTicketTypes = async () => {
        try {
            const data = await getTicketTypes();
            setTicketTypes(data);
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchServices();
        fetchFlights();
        fetchTicketTypes();
    }, []);

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
            return { tickets: [], total: 0 };
        }
    };

    const addItem = async (item: any) => {
        try {
            const ticket: TicketRequest = {
                ticketType: parseInt(item.ticketType, 10),
                price: item.price,
                seat: item.seat,
                flightId: item.flightId,
                services: item.services.map((serviceId: number) => ({ serviceId })),
            };
            console.log(ticket);
            const newTicket = await createTicket(ticket);
            return newTicket;
        } catch (error) {
            throw error;
        }
    };

    const updateItem = async (item: any) => {
        try {
            const ticket: TicketRequest = {
                ticketType: parseInt(item.ticketType, 10),
                price: item.price,
                seat: item.seat,
                flightId: item.flightId,
                services: item.services.map((serviceId: number) => ({ serviceId })),
            };
            const updatedTicket = await updateTicket(item.id, ticket);
            return updatedTicket;
        } catch (error) {
            throw error;
        }
    };

    const deleteItem = async (id: number) => {
        try {
            await deleteTicket(id);
        } catch (error) {
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
                services: [],
            }}
            initialSortBy="id"
            initialDescending={false}
            services={services}
            flights={flights}
            ticketTypes={ticketTypes}

        />
    );
};

export default TicketPage;