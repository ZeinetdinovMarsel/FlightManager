import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { getAirports, deleteAirport, updateAirport, createAirport } from "../api/airport";
import { getFederalDistricts } from "../api/federalDistrict";
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
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { useSettingsStore } from "../store/settings";

const AirportPage: React.FC = () => {
    const { t } = useTranslation();
    const [airports, setAirports] = useState<any[]>([]);
    const [federalDistricts, setFederalDistricts] = useState<any[]>([]);
    const [federalDistrictsMap, setFederalDistrictsMap] = useState<{ [key: number]: string }>({});
    const [page, setPage] = useState(0);
    const [currRowsPerPage, setCurrRowsPerPage] = useState<number>();
    const [sortBy, setSortBy] = useState<any>(null);
    const [descending, setDescending] = useState<boolean>(false);
    const [confirmDelete, setConfirmDelete] = useState<{ open: boolean; id: number | null }>({ open: false, id: null });
    const [editDialog, setEditDialog] = useState<{ open: boolean; airport: any | null }>({ open: false, airport: null });
    const [addDialog, setAddDialog] = useState<{ open: boolean; airport: any }>({ open: false, airport: { name: "", city: "", federalDistrictId: "" } });
    const [error, setError] = useState<string | null>(null);
    const [hoveredRow, setHoveredRow] = useState<number | null>(null);

    const [nameFilter, setNameFilter] = useState<string>("");
    const [cityFilter, setCityFilter] = useState<string>("");
    const [federalDistrictFilter, setFederalDistrictFilter] = useState<any>(null);

    const { rowsPerPage: settingsRowsPerPage, updateTime } = useSettingsStore();

    useEffect(() => {
        setCurrRowsPerPage(settingsRowsPerPage);
    }, [settingsRowsPerPage]);

    useEffect(() => {
        setPage(page);
    }, [page]);

    useEffect(() => {
        const fetchAirports = async () => {
            try {
                const data = await getAirports(
                    sortBy,
                    descending,
                    page + 1,
                    currRowsPerPage,
                    cityFilter,
                    nameFilter,
                    federalDistrictFilter
                );
                setAirports(data);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchAirports();
    }, [sortBy, descending, page, currRowsPerPage, nameFilter, cityFilter, federalDistrictFilter]);

    useEffect(() => {
        const fetchFederalDistricts = async () => {
            try {
                const data = await getFederalDistricts();
                setFederalDistricts(data);
                const federalDistrictsMap = data.reduce((acc: { [x: string]: any; }, district: { id: string | number; name: any; }) => {
                    acc[district.id] = district.name;
                    return acc;
                }, {});
                setFederalDistrictsMap(federalDistrictsMap);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchFederalDistricts();
    }, []);

    useEffect(() => {
        const interval = setInterval(() => {
            const fetchAirports = async () => {
                try {
                    const data = await getAirports(
                        sortBy,
                        descending,
                        page + 1,
                        currRowsPerPage,
                        cityFilter,
                        nameFilter,
                        federalDistrictFilter
                    );
                    setAirports(data);
                } catch (err: any) {
                    setError(err.response?.data);
                }
            };
            fetchAirports();
        }, updateTime * 1000);
    
        return () => clearInterval(interval);
    }, [updateTime, sortBy, descending, page, currRowsPerPage, cityFilter, nameFilter, federalDistrictFilter]);

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
                await deleteAirport(confirmDelete.id);
                setAirports(airports.filter((airport) => airport.id !== confirmDelete.id));
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setConfirmDelete({ open: false, id: null });
    };

    const handleEditOpen = (airport: any) => {
        setEditDialog({ open: true, airport: airport });
    };

    const handleEditSave = async () => {
        if (editDialog.airport) {
            try {
                await updateAirport(
                    editDialog.airport.id,
                    editDialog.airport.name,
                    editDialog.airport.city,
                    editDialog.airport.federalDistrictId || 0
                );
                setAirports(
                    airports.map((a) => (a.id === editDialog.airport.id ? editDialog.airport : a))
                );
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setEditDialog({ open: false, airport: null });
    };

    const handleAddOpen = () => {
        setAddDialog({ open: true, airport: { name: "", city: "", federalDistrictId: "" } });
    };

    const handleAddSave = async () => {
        try {
            const airport = await createAirport(
                addDialog.airport.name,
                addDialog.airport.city,
                addDialog.airport.federalDistrictId || 0
            );
            airports.push(airport);
            setAirports(
                airports.map((a) => (a.id === addDialog.airport.id ? addDialog.airport : a))
            );
            setAddDialog({ open: false, airport: { name: "", city: "", federalDistrictId: "" } });
        } catch (err: any) {
            setError(err.response?.data);
        }
    };

    const handleFilterChange = (field: string, value: any) => {
        switch (field) {
            case "name":
                setNameFilter(value);
                break;
            case "city":
                setCityFilter(value);
                break;
            case "federalDistrictId":
                setFederalDistrictFilter(value === "" ? null : value);
                break;
            default:
                break;
        }
    };

    return (
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
                        {t("addAirport")}
                    </Button>
                </div>
                <div>
                    <TextField
                        label={t("filterByName")}
                        value={nameFilter}
                        onChange={(e) => handleFilterChange("name", e.target.value)}
                        variant="outlined"
                        size="medium"
                        style={{ margin: "10px" }}
                    />
                    <TextField
                        label={t("filterByCity")}
                        value={cityFilter}
                        onChange={(e) => handleFilterChange("city", e.target.value)}
                        variant="outlined"
                        size="medium"
                        style={{ margin: "10px" }}
                    />
                    <FormControl variant="outlined" size="medium" style={{ margin: "10px",width: "200px" }}>
                        <InputLabel>{t("filterByFederalDistrict")}</InputLabel>
                        <Select
                            value={federalDistrictFilter}
                            onChange={(e) => handleFilterChange("federalDistrictId", e.target.value)}
                            label={t("filterByFederalDistrict")}
                        >
                            <MenuItem value="">{t("all")}</MenuItem>
                            {federalDistricts.map((district) => (
                                <MenuItem key={district.id} value={district.id}>
                                    {district.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </div>
            </div>
            <div style={{ margin: "10px" }}>
                <strong>{t("totalRecords")}: {airports.length}</strong>
            </div>
            <h2>{t("airportsTable")}</h2>

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
                                active={sortBy === "name"}
                                direction={descending ? "desc" : "asc"}
                                onClick={() => handleSort("name")}
                            >
                                {t("airports")}
                            </TableSortLabel>
                        </TableCell>
                        <TableCell>
                            <TableSortLabel
                                active={sortBy === "city"}
                                direction={descending ? "desc" : "asc"}
                                onClick={() => handleSort("city")}
                            >
                                {t("city")}
                            </TableSortLabel>
                        </TableCell>
                        <TableCell>
                            <TableSortLabel
                                active={sortBy === "federaldistrict"}
                                direction={descending ? "desc" : "asc"}
                                onClick={() => handleSort("federaldistrict")}
                            >
                                {t("federalDistrict")}
                            </TableSortLabel>
                        </TableCell>
                        <TableCell>{t("actions")}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {airports.map((airport) => (
                        <TableRow
                            key={airport.id}
                            style={{
                                backgroundColor:
                                    airport.id % 2 === 0 ? "#f9f9f9" : "#ffffff",
                                cursor: "pointer",
                                backgroundColor:
                                    hoveredRow === airport.id
                                        ? "#d3e0e9"
                                        : airport.id % 2 === 0
                                            ? "#f9f9f9"
                                            : "#ffffff",
                            }}
                            hover
                            onMouseEnter={() => setHoveredRow(airport.id)}
                            onMouseLeave={() => setHoveredRow(null)}
                        >
                            <TableCell style={{ textAlign: "center" }}>
                                {airport.id}
                            </TableCell>
                            <TableCell>{airport.name}</TableCell>
                            <TableCell>{airport.city}</TableCell>
                            <TableCell>
                                {federalDistrictsMap[airport.federalDistrictId] || t("unknown")}
                            </TableCell>
                            <TableCell>
                                <IconButton onClick={() => handleDeleteConfirm(airport.id)}>
                                    <DeleteIcon color="error" />
                                </IconButton>
                                <IconButton onClick={() => handleEditOpen(airport)}>
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
                onPageChange={(e, newPage) => setPage(newPage)}
                rowsPerPage={currRowsPerPage ?? 10}
                rowsPerPageOptions={Array.from({ length: 51 }, (_, i) => i)}
                onRowsPerPageChange={(e) => {
                    const value = parseInt(e.target.value, 10);
                    setCurrRowsPerPage(value);
                }}
            />

            <Dialog open={addDialog.open} onClose={() => setAddDialog({ open: false, airport: { name: "", city: "", federalDistrictId: "" } })}>
                <DialogTitle>{t("addAirport")}</DialogTitle>
                <DialogContent style={{ padding: "20px" }}>
                    <TextField
                        label={t("name")}
                        fullWidth
                        value={addDialog.airport.name}
                        onChange={(e) =>
                            setAddDialog({
                                ...addDialog,
                                airport: { ...addDialog.airport, name: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }}
                    />
                    <TextField
                        label={t("city")}
                        fullWidth
                        value={addDialog.airport.city}
                        onChange={(e) =>
                            setAddDialog({
                                ...addDialog,
                                airport: { ...addDialog.airport, city: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }
                        }
                    />
                    <FormControl fullWidth sx={{ marginBottom: 2 }}>
                        <InputLabel>{t("federalDistrict")}</InputLabel>
                        <Select
                            value={addDialog.airport.federalDistrictId}
                            onChange={(e) =>
                                setAddDialog({
                                    ...addDialog,
                                    airport: { ...addDialog.airport, federalDistrictId: e.target.value },
                                })
                            }
                            label={t("federalDistrict")}
                        >
                            {federalDistricts.map((district) => (
                                <MenuItem key={district.id} value={district.id}>
                                    {district.name}
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
                                airport: { name: "", city: "", federalDistrictId: "" },
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

            <Dialog open={editDialog.open} onClose={() => setEditDialog({ open: false, airport: null })}>
                <DialogTitle>{t("editAirport")}</DialogTitle>
                <DialogContent style={{ padding: "20px" }}>
                    <TextField
                        label={t("name")}
                        fullWidth
                        value={editDialog.airport?.name || ""}
                        onChange={(e) =>
                            setEditDialog({
                                ...editDialog,
                                airport: { ...editDialog.airport, name: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }
                        }
                    />
                    <TextField
                        label={t("city")}
                        fullWidth
                        value={editDialog.airport?.city || ""}
                        onChange={(e) =>
                            setEditDialog({
                                ...editDialog,
                                airport: { ...editDialog.airport, city: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }
                        }
                    />
                    <FormControl fullWidth sx={{ marginBottom: 2 }}>
                        <InputLabel>{t("federalDistrict")}</InputLabel>
                        <Select
                            value={editDialog.airport?.federalDistrictId}
                            onChange={(e) =>
                                setEditDialog({
                                    ...editDialog,
                                    airport: { ...editDialog.airport, federalDistrictId: e.target.value },
                                })
                            }
                            label={t("federalDistrict")
                            }
                        >
                            {federalDistricts.map((district) => (
                                <MenuItem key={district.id} value={district.id}>
                                    {district.name}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setEditDialog({ open: false, airport: null })}>
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
    );
};

export default AirportPage;