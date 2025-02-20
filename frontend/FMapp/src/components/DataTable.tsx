// src/components/DataTable.tsx
import React, { useEffect, useState } from 'react';
import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Paper,
    TablePagination,
    TableSortLabel,
    IconButton,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Snackbar,
    Alert,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Checkbox,
    ListItemText,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { LocalizationProvider, DateTimePicker } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { useTranslation } from 'react-i18next';
import { format } from 'date-fns';
import { useSettingsStore } from "../store/settings";
import { TicketType } from '../api/ticket';
import ImagePopup from './ImagePopup';
import loadingGif from '../assets/loader.gif'

interface Column {
    id: string;
    label: string;
    sortable?: boolean;
    render?: (row: any) => React.ReactNode;
    type?: 'date' | 'text' | 'number' | 'select' | 'multiselect' | 'imageUrl';
}

interface DataTableProps {
    tableName: string;
    columns: Column[];
    fetchData: (sortBy: string, descending: boolean, page: number, pageSize: number, filters: any) => Promise<any[]>;
    addItem: (item: any) => Promise<any>;
    updateItem: (item: any) => Promise<void>;
    deleteItem: (id: number) => Promise<void>;
    filters?: { [key: string]: any };
    initialSortBy?: string;
    initialDescending?: boolean;
    rowsPerPageOptions?: number[];
    initialRowsPerPage?: number;
    airports?: { id: number; name: string }[];
    federalDistricts?: { id: number; name: string }[];
    flights?: { id: number; flightNumber: string }[];
    services?: { id: number; name: string; cost: number }[];
    ticketTypes?: TicketType[];
}

const DataTable: React.FC<DataTableProps> = ({
    tableName,
    columns,
    fetchData,
    addItem,
    updateItem,
    deleteItem,
    filters = {},
    initialSortBy = '',
    initialDescending = false,
    rowsPerPageOptions = Array.from({ length: 51 }, (_, i) => i),
    airports = [],
    federalDistricts = [],
    flights = [],
    services = [],
    ticketTypes = [],
}) => {
    const { rowsPerPage: settingsRowsPerPage, updateTime } = useSettingsStore();
    const { t } = useTranslation();
    const [data, setData] = useState<any[]>([]);
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(settingsRowsPerPage);
    const [sortBy, setSortBy] = useState<string | null>(initialSortBy);
    const [descending, setDescending] = useState<boolean>(initialDescending);
    const [confirmDelete, setConfirmDelete] = useState<{ open: boolean; id: number | null }>({ open: false, id: null });
    const [dialogOpen, setDialogOpen] = useState<boolean>(false);
    const [dialogMode, setDialogMode] = useState<'add' | 'edit'>('add');
    const [dialogItem, setDialogItem] = useState<any>({});
    const [error, setError] = useState<string | null>(null);
    const [hoveredRow, setHoveredRow] = useState<number | null>(null);
    const [filtersState, setFiltersState] = useState<any>(filters);
    const [loading, setLoading] = useState<boolean>(true);


    const [imageUrl, setImageUrl] = useState<string | null>(null);
    const [showPopup, setShowPopup] = useState<boolean>(false);

    useEffect(() => {
        const fetchDataAsync = async () => {
            try {
                const data = await fetchData(sortBy, descending, page + 1, rowsPerPage, filtersState);
                setData(data);
            } catch (err: any) {
                setError(err.response?.data || err.message);
            } finally {
                setLoading(false);
            }
        };
        fetchDataAsync();
    }, [sortBy, descending, page, rowsPerPage, filtersState]);

    useEffect(() => {
        const interval = setInterval(() => {
            const fetchFlights = async () => {
                try {
                    const data = await fetchData(sortBy, descending, page + 1, rowsPerPage, filtersState);
                    setData(data);
                } catch (err: any) {
                    setError(err.response?.data);
                }
            };
            fetchFlights();
        }, updateTime * 1000);

        return () => clearInterval(interval);
    }, [updateTime, sortBy, descending, page, rowsPerPage, rowsPerPage, filtersState]);

    const handleSort = (field: string) => {
        if (sortBy === field) {
            setDescending(!descending);
        } else {
            setSortBy(field);
            setDescending(false);
        }
    };

    const handleDeleteConfirm = (id: number) => {
        setConfirmDelete({ open: true, id });
    };

    const handleDelete = async () => {
        if (confirmDelete.id !== null) {
            try {
                await deleteItem(confirmDelete.id);
                setData(data.filter((item) => item.id !== confirmDelete.id));
            } catch (err: any) {
                setError(err.response?.data || err.message);
            }
        }
        setConfirmDelete({ open: false, id: null });
    };

    const handleEditOpen = (item: any) => {
        const updatedItem = { ...item };
        columns.forEach((col) => {
            if (col.type === 'date' && typeof updatedItem[col.id] === 'string') {
                updatedItem[col.id] = new Date(updatedItem[col.id]);
            }
        });

        if (Array.isArray(updatedItem.services)) {
            updatedItem.services = updatedItem.services.map((service: any) => service.serviceId);
        }
        setDialogItem(updatedItem);
        setDialogMode('edit');
        setDialogOpen(true);
    };


    const handleAddOpen = () => {
        setDialogMode('add');
        setDialogItem({});
        setDialogOpen(true);
    };

    const handleDialogSave = async () => {
        try {
            if (dialogMode === 'add') {
                await addItem(dialogItem);
            } else {
                await updateItem(dialogItem);
            }
            const data = await fetchData(sortBy, descending, page + 1, rowsPerPage, filtersState);
            setData(data);
            setDialogOpen(false);
        } catch (err: any) {
            setError(err.response?.data || err.message);
        }
    };

    const handleDialogChange = (field: string, value: any) => {
        setDialogItem({ ...dialogItem, [field]: value });
    };

    const handleFilterChange = (field: string, value: any) => {
        setFiltersState({ ...filtersState, [field]: value });
        setPage(0);
    };

    const handlePageChange = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleRowsPerPageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const formatDate = (date: any) => {
        if (date instanceof Date) {
            return format(date, 'dd-MM-yyyy HH:mm');
        } else if (typeof date === 'string') {
            const parsedDate = new Date(date);
            if (!isNaN(parsedDate.getTime())) {
                return format(parsedDate, 'dd-MM-yyyy HH:mm');
            }
        }
        return 'N/A';
    };

    const getAirportName = (id: number) => {
        const airport = airports.find((airport) => airport.id === id);
        return airport ? airport.name : 'N/A';
    };

    const getFederalDistrictName = (id: number) => {
        const district = federalDistricts.find((district) => district.id === id);
        return district ? district.name : 'N/A';
    };

    const getServiceName = (servicess: any[]) => {
        if (Array.isArray(servicess)) {
            return servicess.map((service: any) => {
                const foundService = services.find((s) => s.id === service.serviceId);
                return foundService ? foundService.name : 'N/A';
            }).join(', ');
        }
        return '';
    };

    const getFlightNumber = (id: number) => {

        const foundFlight = flights.find((flight) => flight.id === id);
        return foundFlight ? foundFlight.flightNumber : 'N/A';

    };

    const getTicketTypeName = (id: number) => {

        const foundTicketType = ticketTypes.find((ticketType) => ticketType.id === id);
        return foundTicketType ? foundTicketType.name : 'N/A';

    };

    const getFilterComponent = (col, value, onChange, t, dialog) => {
        const handleChange = (newValue) => {
            if ((col.type === 'select' || col.type === 'date') && (!newValue || newValue === '')) {
                onChange(col.id, null);
            } else {
                onChange(col.id, newValue);
            }
        };

        switch (col.type) {
            case 'date':
                return (
                    <LocalizationProvider dateAdapter={AdapterDateFns} key={col.id}>
                        <DateTimePicker
                            label={dialog ? t(col.label) : `${t("filterBy")} ${t(col.label)}`}
                            value={value || null}
                            onChange={handleChange}
                            ampm={false}
                            format="dd-MM-yyyy HH:mm"
                            sx={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "250px" }}
                        />
                    </LocalizationProvider>
                );
            case 'multiselect':
                return (
                    <FormControl key={col.id} sx={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "250px" }}>
                        <InputLabel>{dialog ? t(col.label) : `${t("filterBy")} ${t(col.label)}`}</InputLabel>
                        <Select
                            multiple
                            value={value || []}
                            onChange={(e) => {
                                const selected = e.target.value;
                                if (selected.length === 0) {
                                    onChange(col.id, []);
                                } else {
                                    onChange(col.id, selected);
                                }
                            }}
                            renderValue={(selected) =>
                                (selected as number[])
                                    .map((id) => services.find((service) => service.id === id)?.name || '')
                                    .join(', ')
                            }
                        >
                            {services.map((service) => (
                                <MenuItem key={service.id} value={service.id}>
                                    <Checkbox checked={value?.includes(service.id) || false} />
                                    <ListItemText primary={service.name} />
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                );
            case 'select':
                return (
                    <FormControl key={col.id} sx={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "250px" }}>
                        <InputLabel>{dialog ? t(col.label) : `${t("filterBy")} ${t(col.label)}`}</InputLabel>
                        <Select
                            value={value !== undefined ? value : ''}
                            onChange={(e) => {
                                const selected = e.target.value;
                                if (!selected) {
                                    onChange(col.id, null);
                                } else {
                                    onChange(col.id, selected);
                                }
                            }}
                        >
                            <MenuItem value="">
                                <em>{t("none")}</em>
                            </MenuItem>
                            {(col.id === 'airportId'
                                ? airports
                                : col.id === 'flightId'
                                    ? flights
                                    : col.id === "ticketType"
                                        ? ticketTypes
                                        : federalDistricts).map((item) => (
                                            <MenuItem key={item.id} value={item.id}>
                                                {item.name || item.flightNumber}
                                            </MenuItem>
                                        ))}
                        </Select>
                    </FormControl>
                );

            case 'imageUrl':
                return (dialog ?
                    <div key={col.id} style={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "0px" }}>
                        <TextField
                            label={dialog ? t(col.label) : `${t("filterBy")} ${t(col.label)}`}
                            value={value !== undefined ? value : ''}
                            onChange={(e) => {
                                const newValue = e.target.value;
                                onChange(col.id, newValue);
                            }}
                            sx={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "250px" }}
                            type="text"
                        />
                        {value && (
                            <div style={{ marginBottom: 2 }}>
                                <Button variant="outlined" sx={{ marginBottom: 2 }} onClick={() => {
                                    setImageUrl(value);
                                    setShowPopup(true);
                                }}>
                                    {t("viewImage")}
                                </Button>
                            </div>
                        )}
                    </div>
                    : null
                );
            default:
                return (
                    <TextField
                        key={col.id}
                        label={dialog ? t(col.label) : `${t("filterBy")} ${t(col.label)}`}
                        value={value !== undefined ? value : ''}
                        onChange={(e) => {
                            const newValue = e.target.value;
                            if (newValue === '') {
                                onChange(col.id, null);
                            } else {
                                onChange(col.id, newValue);
                            }
                        }}
                        sx={{ margin: dialog ? 0 : 1, marginBottom: 2, width: dialog ? "100%" : "250px" }}
                        type={col.type === 'number' ? 'number' : 'text'}
                    />
                );
        }
    };

    const handleClosePopup = () => {
        setShowPopup(false);
        setImageUrl(null);
    };


    const getCellContent = (col, item, index) => {
        if (col.label === 'id') {
            return index + 1;
        }
        else if (col.id === 'airplanePhotoUrl') {
            return (
                <a href={item[col.id]} target="_blank" rel="noopener noreferrer" onClick={(e) => {
                    e.preventDefault();
                    setImageUrl(item[col.id]);
                    setShowPopup(true);
                }}>
                    {t("viewImage")}
                </a>
            );
        } else if (col.type === 'date') {
            return formatDate(item[col.id]);
        } else if (col.id === 'services') {
            return getServiceName(item[col.id]);
        } else if (col.id === 'airportId') {
            return getAirportName(item[col.id]);
        } else if (col.id === 'federalDistrictId') {
            return getFederalDistrictName(item[col.id]);
        } else if (col.id === 'flightId') {
            return getFlightNumber(item[col.id]);
        } else if (col.id === 'ticketType') {
            return getTicketTypeName(item[col.id]);
        } else {
            return item[col.id] !== undefined && item[col.id] !== null ? item[col.id] : 'N/A';
        }
    };
    return (
        <LocalizationProvider dateAdapter={AdapterDateFns}>
            <TableContainer component={Paper}>
                <div
                    style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        margin: "10px",
                    }}
                >
                    <div>
                        <Button onClick={handleAddOpen} color="primary" variant="contained" style={{ margin: "10px" }}>
                            {t("addItem")}
                        </Button>
                    </div>
                    <div>
                        {columns
                            .filter((col) => col.id !== 'actions' && col.id !== 'id')
                            .map((col) => (
                                getFilterComponent(col, filtersState[col.id], handleFilterChange, t, false)
                            ))}
                    </div>
                </div>
                <div style={{ margin: "10px" }}>
                    <strong>{t("totalRecords")}: {data.length}</strong>
                </div>
                <h2 style={{ textAlign: 'center' }}>{t(`${tableName}`)}</h2>

                {loading ? (
                    <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: '20px' }}>
                        <img src={loadingGif} alt="Loading" style={{ width: '5%', height: 'auto' }} />
                    </div>
                ) : (
                    <Table>
                        <TableHead>
                            <TableRow style={{ backgroundColor: '#005da8', color: '#fff' }}>
                                {columns.map((col) => (
                                    <TableCell key={col.id} align="center" style={{ fontWeight: 'bold', color: '#fff' }}>
                                        {col.sortable ? (
                                            <TableSortLabel
                                                active={sortBy === col.id}
                                                direction={descending ? "desc" : "asc"}
                                                onClick={() => handleSort(col.id)}
                                                style={{ color: '#fff' }}
                                            >
                                                {t(col.label)}
                                            </TableSortLabel>
                                        ) : (
                                            t(col.label)
                                        )}
                                    </TableCell>
                                ))}
                                <TableCell style={{ color: '#fff' }}>{t("actions")}</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {data.length === 0 ? (
                                <TableRow>
                                    <TableCell colSpan={columns.length + 1} align="center">No data available</TableCell>
                                </TableRow>
                            ) : (
                                data.map((item, index) => (
                                    <TableRow
                                        key={item.id}
                                        style={{
                                            backgroundColor:
                                                index % 2 === 0 ? "#f9f9f9" : "#ffffff",
                                            cursor: "pointer",
                                            backgroundColor:
                                                hoveredRow === item.id
                                                    ? "#d3e0e9"
                                                    : index % 2 === 1
                                                        ? "#f9f9f9"
                                                        : "#ffffff",
                                        }}
                                        hover
                                        onMouseEnter={() => setHoveredRow(item.id)}
                                        onMouseLeave={() => setHoveredRow(null)}
                                    >
                                        {columns.map((col) => (
                                            <TableCell key={col.id} align={(col.type === 'number' || col.type === 'date' || col.type === 'imageUrl') ? 'center' : 'left'}>
                                                {getCellContent(col, item, index)}
                                            </TableCell>
                                        ))}
                                        <TableCell>
                                            <IconButton onClick={() => handleDeleteConfirm(item.id)}>
                                                <DeleteIcon color="error" />
                                            </IconButton>
                                            <IconButton onClick={() => handleEditOpen(item)}>
                                                <EditIcon color="primary" />
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                ))
                            )}
                        </TableBody>
                    </Table>
                )}

                <TablePagination
                    component="div"
                    count={100}
                    page={page}
                    onPageChange={handlePageChange}
                    rowsPerPage={rowsPerPage}
                    rowsPerPageOptions={rowsPerPageOptions.map((size) => size)}
                    onRowsPerPageChange={handleRowsPerPageChange}
                />

                <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
                    <DialogTitle>{dialogMode === 'add' ? t("addItem") : t("editItem")}</DialogTitle>
                    <DialogContent style={{ padding: "20px" }}>
                        {columns
                            .filter((col) => col.id !== 'id' && col.id !== 'actions')
                            .map((col) => (
                                getFilterComponent(col, dialogItem[col.id], handleDialogChange, t, true)
                            ))}
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setDialogOpen(false)} color="primary">
                            {t("cancel")}
                        </Button>
                        <Button onClick={handleDialogSave} color="primary" disabled={!dialogItem}>
                            {t("save")}
                        </Button>
                    </DialogActions>
                </Dialog>

                <Dialog
                    open={confirmDelete.open}
                    onClose={() => setConfirmDelete({ open: false, id: null })}
                >
                    <DialogTitle>{t("confirmDelete")}</DialogTitle>
                    <DialogActions>
                        <Button onClick={() => setConfirmDelete({ open: false, id: null })}>
                            {t("cancel")}
                        </Button>
                        <Button onClick={handleDelete} color="error">
                            {t("delete")}
                        </Button>
                    </DialogActions>
                </Dialog>
                <ImagePopup open={showPopup} imageUrl={imageUrl} onClose={handleClosePopup} />
                <Snackbar
                    open={!!error}
                    autoHideDuration={6000}
                    onClose={() => setError(null)}
                >
                    <Alert onClose={() => setError(null)} severity="error">
                        {error}
                    </Alert>
                </Snackbar>
            </TableContainer>
        </LocalizationProvider>
    );
};

export default DataTable;