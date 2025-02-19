// src/pages/FlightsPage.tsx

import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import {
    getFlights,
    deleteFlight,
    updateFlight,
    createFlight,
} from "../api/flight";
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
    Select,
    MenuItem,
    InputLabel,
    FormControl,
    Snackbar,
    Alert,
    Grid,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { DateTimePicker, LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { useSettingsStore } from "../store/settings";
import { getAirports } from "../api/airport"; 
import { format } from "date-fns";

const FlightsPage: React.FC = () => {
    const { t } = useTranslation();
    const [flights, setFlights] = useState<any[]>([]);
    const [airports, setAirports] = useState<any[]>([]);
    const [airportsMap, setAirportsMap] = useState<{ [key: number]: string }>({});
    const [page, setPage] = useState(0);
    const [currRowsPerPage, setCurrRowsPerPage] = useState<number>(10);
    const [sortBy, setSortBy] = useState<any>(null);
    const [descending, setDescending] = useState<boolean>(false);
    const [confirmDelete, setConfirmDelete] = useState<{ open: boolean; id: number | null }>({ open: false, id: null });
    const [editDialog, setEditDialog] = useState<{ open: boolean; flight: any | null }>({ open: false, flight: null });
    const [addDialog, setAddDialog] = useState<{ open: boolean; flight: any }>({
        open: false,
        flight: {
            flightNumber: "",
            destination: "",
            departureTime: null,
            arrivalTime: null,
            availableSeats: 0,
            airplanePhotoUrl: "",
            airportId: 0
        }
    });
    const [error, setError] = useState<string | null>(null);
    const [hoveredRow, setHoveredRow] = useState<number | null>(null);

    const [flightNumberFilter, setFlightNumberFilter] = useState<string>("");
    const [destinationFilter, setDestinationFilter] = useState<string>("");
    const [departureTimeFilter, setDepartureTimeFilter] = useState<Date | null>(null);
    const [arrivalTimeFilter, setArrivalTimeFilter] = useState<Date | null>(null);
    const [availableSeatsFilter, setAvailableSeatsFilter] = useState<any | null>(null);
    const [airportIdFilter, setAirportIdFilter] = useState<number | null>(null);

    const { rowsPerPage: settingsRowsPerPage, updateTime } = useSettingsStore();

    useEffect(() => {
        const fetchAirports = async () => {
            try {
                const data = await getAirports();
                setAirports(data);

                const federalDistrictsMap = data.reduce((acc: { [x: string]: any; }, district: { id: string | number; name: any; }) => {
                    acc[district.id] = district.name;
                    return acc;
                }, {});
                setAirportsMap(federalDistrictsMap);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchAirports();
    }, []);

    useEffect(() => {
        setCurrRowsPerPage(settingsRowsPerPage);
    }, [settingsRowsPerPage]);

    useEffect(() => {
        const fetchFlights = async () => {
            try {
                const data = await getFlights(
                    sortBy,
                    descending,
                    page + 1,
                    currRowsPerPage,
                    flightNumberFilter,
                    destinationFilter,
                    departureTimeFilter ? formatDateTimeForServer(departureTimeFilter) : null,
                    arrivalTimeFilter ? formatDateTimeForServer(arrivalTimeFilter) : null,
                    availableSeatsFilter,
                    airportIdFilter
                );
                setFlights(data);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchFlights();
    }, [sortBy, descending, page, currRowsPerPage, flightNumberFilter, destinationFilter, departureTimeFilter, arrivalTimeFilter, availableSeatsFilter, airportIdFilter]);

    useEffect(() => {
        const interval = setInterval(() => {
            const fetchFlights = async () => {
                try {
                    const data = await getFlights(
                        sortBy,
                        descending,
                        page + 1,
                        currRowsPerPage,
                        flightNumberFilter,
                        destinationFilter,
                        departureTimeFilter ? formatDateTimeForServer(departureTimeFilter) : null,
                        arrivalTimeFilter ? formatDateTimeForServer(arrivalTimeFilter) : null,
                        availableSeatsFilter,
                        airportIdFilter
                    );
                    setFlights(data);
                } catch (err: any) {
                    setError(err.response?.data);
                }
            };
            fetchFlights();
        }, updateTime * 1000);

        return () => clearInterval(interval);
    }, [updateTime, sortBy, descending, page, currRowsPerPage, flightNumberFilter, destinationFilter, departureTimeFilter, arrivalTimeFilter, availableSeatsFilter, airportIdFilter]);

    const handleSort = (field: string) => {
        setDescending(sortBy === field ? !descending : false);
        setSortBy(field);
    };

    const handleDeleteConfirm = (id: number) => {
        setConfirmDelete({ open: true, id });
    };

    const handleDelete = async () => {
        if (confirmDelete.id !== null) {
            try {
                await deleteFlight(confirmDelete.id);
                setFlights(flights.filter((flight) => flight.id !== confirmDelete.id));
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setConfirmDelete({ open: false, id: null });
    };

    const handleEditOpen = (flight: any) => {
        const departureTime = flight.departureTime ? new Date(flight.departureTime) : null;
        const arrivalTime = flight.arrivalTime ? new Date(flight.arrivalTime) : null;
        setEditDialog({ open: true, flight: { ...flight, departureTime, arrivalTime } });
    };

    const handleEditSave = async () => {
        if (editDialog.flight) {
            try {
                const departureTime = formatDateTimeForServer(editDialog.flight.departureTime);
                const arrivalTime = formatDateTimeForServer(editDialog.flight.arrivalTime);

                await updateFlight(
                    editDialog.flight.id,
                    editDialog.flight.flightNumber,
                    editDialog.flight.destination,
                    departureTime,
                    arrivalTime,
                    editDialog.flight.availableSeats,
                    editDialog.flight.airplanePhotoUrl,
                    editDialog.flight.airportId
                );
                setFlights(
                    flights.map((f) => (f.id === editDialog.flight.id ? editDialog.flight : f))
                );
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setEditDialog({ open: false, flight: null });
    };

    const handleAddOpen = () => {
        setAddDialog({
            open: true,
            flight: {
                flightNumber: "",
                destination: "",
                departureTime: null,
                arrivalTime: null,
                availableSeats: 0,
                airplanePhotoUrl: "",
                airportId: 0
            }
        });
    };

    const handleAddSave = async () => {
        try {
            const departureTime = formatDateTimeForServer(addDialog.flight.departureTime);
            const arrivalTime = formatDateTimeForServer(addDialog.flight.arrivalTime);

            const flight = await createFlight(
                addDialog.flight.flightNumber,
                addDialog.flight.destination,
                departureTime,
                arrivalTime,
                addDialog.flight.availableSeats,
                addDialog.flight.airplanePhotoUrl,
                addDialog.flight.airportId
            );
            flights.push(flight);
            setFlights([...flights]);
            setAddDialog({
                open: false,
                flight: {
                    flightNumber: "",
                    destination: "",
                    departureTime: null,
                    arrivalTime: null,
                    availableSeats: 0,
                    airplanePhotoUrl: "",
                    airportId: 0
                }
            });
        } catch (err: any) {
            setError(err.response?.data || err.message);
        }
    };

    const handleFilterChange = (field: string, value: any) => {
        switch (field) {
            case "flightNumber":
                setFlightNumberFilter(value);
                break;
            case "destination":
                setDestinationFilter(value);
                break;
            case "departureTime":
                setDepartureTimeFilter(value);
                break;
            case "arrivalTime":
                setArrivalTimeFilter(value);
                break;
            case "availableSeats":
                setAvailableSeatsFilter(value === "" ? null : parseInt(value));
                break;
            case "airportId":
                setAirportIdFilter(value === "" ? null : parseInt(value));
                break;
            default:
                break;
        }
    };

    const handlePageChange = (event: unknown, newPage: number) => {
        setPage(newPage);
    };

    const handleRowsPerPageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setCurrRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
    };

    const formatDateTimeForServer = (dateTime: Date | null): string | null => {
        if (!dateTime) return null;
        return dateTime.toISOString();
    };

    const formatDateTimeForDisplay = (dateTimeString: string): string => {
        if (!dateTimeString) return "";
        const date = new Date(dateTimeString);
        try {
            return format(date, 'dd-MM-yyyy HH:mm');
        }
        catch (err: any) {
            return (err.message);
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
                            {t("addFlight")}
                        </Button>
                    </div>
                    <div>
                        <TextField
                            label={t("filterByFlightNumber")}
                            value={flightNumberFilter}
                            onChange={(e) => handleFilterChange("flightNumber", e.target.value)}
                            variant="outlined"
                            size="medium"
                            style={{ margin: "10px" }}
                        />
                        <TextField
                            label={t("filterByDestination")}
                            value={destinationFilter}
                            onChange={(e) => handleFilterChange("destination", e.target.value)}
                            variant="outlined"
                            size="medium"
                            style={{ margin: "10px" }}
                        />
                        <DateTimePicker
                            label={t("filterByDepartureTime")}
                            value={departureTimeFilter}
                            onChange={(newValue) => {
                                setDepartureTimeFilter(newValue);
                            }}
                            ampm={false}
                        
                            format="dd-MM-yyyy HH:mm"
                            sx={{ margin: "10px" }}
                        />
                        <DateTimePicker
                            label={t("filterByArrivalTime")}
                            value={arrivalTimeFilter}
                            onChange={(newValue) => {
                                setArrivalTimeFilter(newValue);
                            }}
                            ampm={false}
                            format="dd-MM-yyyy HH:mm"
                            sx={{ margin: "10px"}}
                        />
                        <TextField
                            label={t("filterByAvailableSeats")}
                            value={availableSeatsFilter}
                            onChange={(e) => handleFilterChange("availableSeats", e.target.value)}
                            variant="outlined"
                            size="medium"
                            type="number"
                            style={{ margin: "10px" }}
                        />
                        <FormControl variant="outlined" size="medium" style={{ margin: "10px", width: "200px"}}>
                            <InputLabel>{t("filterByAirportId")}</InputLabel>
                            <Select
                                value={airportIdFilter}
                                onChange={(e) => handleFilterChange("airportId", e.target.value)}
                                label={t("filterByAirportId")}
                            >
                                <MenuItem value="">{t("all")}</MenuItem>
                                {airports.map((airport) => (
                                    <MenuItem key={airport.id} value={airport.id}>
                                        {airport.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </div>
                </div>
                <div style={{ margin: "10px" }}>
                    <strong>{t("totalRecords")}: {flights.length}</strong>
                </div>
                <h2>{t("flightsTable")}</h2>

                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "id"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("id")}
                                >
                                    {t("id")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "flightnumber"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("flightnumber")}
                                >
                                    {t("flightNumber")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "destination"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("destination")}
                                >
                                    {t("destination")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "departuretime"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("departuretime")}
                                >
                                    {t("departureTime")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "arrivaltime"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("arrivaltime")}
                                >
                                    {t("arrivalTime")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "availableseats"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("availableseats")}
                                >
                                    {t("availableSeats")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "airplanephotourl"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("airplanephotourl")}
                                >
                                    {t("airplanePhotoUrl")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>
                                <TableSortLabel
                                    active={sortBy === "airport"}
                                    direction={descending ? "desc" : "asc"}
                                    onClick={() => handleSort("airport")}
                                >
                                    {t("airportId")}
                                </TableSortLabel>
                            </TableCell>
                            <TableCell>{t("actions")}</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {flights.map((flight) => (
                            <TableRow
                                key={flight.id}
                                style={{
                                    backgroundColor:
                                        flight.id % 2 === 0 ? "#f9f9f9" : "#ffffff",
                                    cursor: "pointer",
                                    backgroundColor:
                                        hoveredRow === flight.id
                                            ? "#d3e0e9"
                                            : flight.id % 2 === 0
                                                ? "#f9f9f9"
                                                : "#ffffff",
                                }}
                                hover
                                onMouseEnter={() => setHoveredRow(flight.id)}
                                onMouseLeave={() => setHoveredRow(null)}
                            >
                                <TableCell style={{ textAlign: "center" }}>
                                    {flight.id}
                                </TableCell>
                                <TableCell>{flight.flightNumber}</TableCell>
                                <TableCell>{flight.destination}</TableCell>
                                <TableCell>{formatDateTimeForDisplay(flight.departureTime)}</TableCell>
                                <TableCell>{formatDateTimeForDisplay(flight.arrivalTime)}</TableCell>
                                <TableCell>{flight.availableSeats}</TableCell>
                                <TableCell>{flight.airplanePhotoUrl}</TableCell>
                                <TableCell>{airportsMap[flight.airportId]}</TableCell>
                                <TableCell>
                                    <IconButton onClick={() => handleDeleteConfirm(flight.id)}>
                                        <DeleteIcon color="error" />
                                    </IconButton>
                                    <IconButton onClick={() => handleEditOpen(flight)}>
                                        <EditIcon color="primary" />
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>

                <TablePagination
                    component="div"
                    count={100}
                    page={page}
                    onPageChange={handlePageChange}
                    rowsPerPage={currRowsPerPage}
                    rowsPerPageOptions={Array.from({ length: 51 }, (_, i) => i)}
                    onRowsPerPageChange={handleRowsPerPageChange}
                />

                <Dialog open={addDialog.open} onClose={() => setAddDialog({ open: false, flight: { flightNumber: "", destination: "", departureTime: null, arrivalTime: null, availableSeats: 0, airplanePhotoUrl: "", airportId: 0 } })}>
                    <DialogTitle>{t("addFlight")}</DialogTitle>
                    <DialogContent style={{ padding: "20px" }}>
                        <TextField
                            label={t("flightNumber")}
                            fullWidth
                            value={addDialog.flight.flightNumber}
                            onChange={(e) =>
                                setAddDialog({
                                    ...addDialog,
                                    flight: { ...addDialog.flight, flightNumber: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <TextField
                            label={t("destination")}
                            fullWidth
                            value={addDialog.flight.destination}
                            onChange={(e) =>
                                setAddDialog({
                                    ...addDialog,
                                    flight: { ...addDialog.flight, destination: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <Grid container spacing={2}  sx={{ marginBottom: 2 }}>
                            <Grid item xs={6}>
                                <DateTimePicker
                                    label={t("departureTime")}
                                    value={addDialog.flight?.departureTime || null}
                                    onChange={(newValue) => {
                                        setEditDialog({
                                            ...addDialog,
                                            flight: { ...addDialog.flight, departureTime: newValue },
                                        });
                                    }}
                                    ampm={false}
                                    format="dd-MM-yyyy HH:mm"
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <DateTimePicker
                                    label={t("arrivalTime")}
                                    value={addDialog.flight?.arrivalTime || null}
                                    onChange={(newValue) => {
                                        setEditDialog({
                                            ...addDialog,
                                            flight: { ...addDialog.flight, arrivalTime: newValue },
                                        });
                                    }}
                                    ampm={false}
                                    format="dd-MM-yyyy HH:mm"
                                    fullWidth
                                />
                            </Grid>
                        </Grid>
                        <TextField
                            label={t("availableSeats")}
                            fullWidth
                            value={addDialog.flight.availableSeats}
                            onChange={(e) =>
                                setAddDialog({
                                    ...addDialog,
                                    flight: { ...addDialog.flight, availableSeats: parseInt(e.target.value) },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                            type="number"
                        />
                        <TextField
                            label={t("airplanePhotoUrl")}
                            fullWidth
                            value={addDialog.flight.airplanePhotoUrl}
                            onChange={(e) =>
                                setAddDialog({
                                    ...addDialog,
                                    flight: { ...addDialog.flight, airplanePhotoUrl: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <FormControl fullWidth sx={{ marginBottom: 2 }}>
                            <InputLabel>{t("airportId")}</InputLabel>
                            <Select
                                value={addDialog.flight.airportId}
                                onChange={(e) =>
                                    setAddDialog({
                                        ...addDialog,
                                        flight: { ...addDialog.flight, airportId: parseInt(e.target.value) },
                                    })
                                }
                                label={t("airportId")}
                            >
                                {airports.map((airport) => (
                                    <MenuItem key={airport.id} value={airport.id}>
                                        {airport.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            onClick={() =>
                                setAddDialog({
                                    open: false,
                                    flight: { flightNumber: "", destination: "", departureTime: null, arrivalTime: null, availableSeats: 0, airplanePhotoUrl: "", airportId: 0 },
                                })
                            }
                            color="primary"
                        >
                            {t("cancel")}
                        </Button>
                        <Button onClick={handleAddSave} color="primary">
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

                <Dialog open={editDialog.open} onClose={() => setEditDialog({ open: false, flight: null })}>
                    <DialogTitle>{t("editFlight")}</DialogTitle>
                    <DialogContent style={{ padding: "20px" }}>
                        <TextField
                            label={t("flightNumber")}
                            fullWidth
                            value={editDialog.flight?.flightNumber || ""}
                            onChange={(e) =>
                                setEditDialog({
                                    ...editDialog,
                                    flight: { ...editDialog.flight, flightNumber: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <TextField
                            label={t("destination")}
                            fullWidth
                            value={editDialog.flight?.destination || ""}
                            onChange={(e) =>
                                setEditDialog({
                                    ...editDialog,
                                    flight: { ...editDialog.flight, destination: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <Grid container spacing={2}  sx={{ marginBottom: 2 }}>
                            <Grid item xs={6}>
                                <DateTimePicker
                                    label={t("departureTime")}
                                    value={editDialog.flight?.departureTime || null}
                                    onChange={(newValue) => {
                                        setEditDialog({
                                            ...editDialog,
                                            flight: { ...editDialog.flight, departureTime: newValue },
                                        });
                                    }}
                                    ampm={false}
                                    format="dd-MM-yyyy HH:mm"
                                    fullWidth
                                />
                            </Grid>
                            <Grid item xs={6}>
                                <DateTimePicker
                                    label={t("arrivalTime")}
                                    value={editDialog.flight?.arrivalTime || null}
                                    onChange={(newValue) => {
                                        setEditDialog({
                                            ...editDialog,
                                            flight: { ...editDialog.flight, arrivalTime: newValue },
                                        });
                                    }}
                                    ampm={false}
                                    format="dd-MM-yyyy HH:mm"
                                    fullWidth
                                />
                            </Grid>
                        </Grid>
                        <TextField
                            label={t("availableSeats")}
                            fullWidth
                            value={editDialog.flight?.availableSeats || ""}
                            onChange={(e) =>
                                setEditDialog({
                                    ...editDialog,
                                    flight: { ...editDialog.flight, availableSeats: parseInt(e.target.value) },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                            type="number"
                        />
                        <TextField
                            label={t("airplanePhotoUrl")}
                            fullWidth
                            value={editDialog.flight?.airplanePhotoUrl || ""}
                            onChange={(e) =>
                                setEditDialog({
                                    ...editDialog,
                                    flight: { ...editDialog.flight, airplanePhotoUrl: e.target.value },
                                })
                            }
                            sx={{ marginBottom: 2 }}
                        />
                        <FormControl fullWidth sx={{ marginBottom: 2 }}>
                            <InputLabel>{t("airportId")}</InputLabel>
                            <Select
                                value={editDialog.flight?.airportId || 0}
                                onChange={(e) =>
                                    setEditDialog({
                                        ...editDialog,
                                        flight: { ...editDialog.flight, airportId: parseInt(e.target.value) },
                                    })
                                }
                                label={t("airportId")}
                            >
                                {airports.map((airport) => (
                                    <MenuItem key={airport.id} value={airport.id}>
                                        {airport.name}
                                    </MenuItem>
                                ))}
                            </Select>
                        </FormControl>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => setEditDialog({ open: false, flight: null })}>
                            {t("cancel")}
                        </Button>
                        <Button onClick={handleEditSave} color="primary">
                            {t("save")}
                        </Button>
                    </DialogActions>
                </Dialog>

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

export default FlightsPage;