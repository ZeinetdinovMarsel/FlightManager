// src/pages/FederalDistrictPage.tsx
import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { getFederalDistricts, deleteFederalDistrict, updateFederalDistrict, createFederalDistrict } from '../api/federalDistrict';
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
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { useSettingsStore } from '../store/settings';

const FederalDistrictPage: React.FC = () => {
    const { t } = useTranslation();

    const [federalDistricts, setFederalDistricts] = useState<any[]>([]);
    const [page, setPage] = useState(0);
    const [currRowsPerPage, setCurrRowsPerPage] = useState<number>();
    const [sortBy, setSortBy] = useState<any>(null);
    const [descending, setDescending] = useState<boolean>(false);
    const [confirmDelete, setConfirmDelete] = useState<{ open: boolean; id: number | null }>({ open: false, id: null });
    const [editDialog, setEditDialog] = useState<{ open: boolean; federalDistrict: any | null }>({ open: false, federalDistrict: null });
    const [addDialog, setAddDialog] = useState<{ open: boolean; federalDistrict: any }>({ open: false, federalDistrict: { name: ""} });
    const [error, setError] = useState<string | null>(null);
    const [hoveredRow, setHoveredRow] = useState<number | null>(null);

    const [nameFilter, setNameFilter] = useState<string>("");

    const { rowsPerPage: settingsRowsPerPage, updateTime } = useSettingsStore();

    useEffect(() => {
        setCurrRowsPerPage(settingsRowsPerPage);
    }, [settingsRowsPerPage]);

    useEffect(() => {
        setPage(page);
    }, [page]);


    useEffect(() => {
        const fetchFederalDistricts = async () => {
            try {
                const data = await getFederalDistricts(
                    sortBy,
                    descending,
                    page + 1,
                    currRowsPerPage,
                    nameFilter,
                );
                setFederalDistricts(data);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchFederalDistricts();
    }, [sortBy, descending, page, currRowsPerPage, nameFilter]);

    useEffect(() => {
        const fetchFederalDistricts = async () => {
            try {
                const data = await getFederalDistricts();
                setFederalDistricts(data);
            } catch (err: any) {
                setError(err.response?.data);
            }
        };
        fetchFederalDistricts();
    }, []);

    useEffect(() => {
        const interval = setInterval(() => {
            const fetchFederalDistricts = async () => {
                try {
                    const data = await getFederalDistricts(
                        sortBy,
                        descending,
                        page + 1,
                        currRowsPerPage,
                        nameFilter,
                    );
                    setFederalDistricts(data);
                } catch (err: any) {
                    setError(err.response?.data);
                }
            };
            fetchFederalDistricts();
        }, updateTime * 1000);

        return () => clearInterval(interval);
    }, [updateTime, sortBy, descending, page, currRowsPerPage, nameFilter]);

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
                await deleteFederalDistrict(confirmDelete.id);
                setFederalDistricts(federalDistricts.filter((federalDistrict) => federalDistrict.id !== confirmDelete.id));
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setConfirmDelete({ open: false, id: null });
    };

    const handleEditOpen = (federalDistrict: any) => {
        setEditDialog({ open: true, federalDistrict: federalDistrict });
    };

    const handleEditSave = async () => {
        if (editDialog.federalDistrict) {
            try {
                await updateFederalDistrict(
                    editDialog.federalDistrict.id,
                    editDialog.federalDistrict.name
                );
                setFederalDistricts(
                    federalDistricts.map((f) => (f.id === editDialog.federalDistrict.id ? editDialog.federalDistrict : f))
                );
            } catch (err: any) {
                setError(err.response?.data);
            }
        }
        setEditDialog({ open: false, federalDistrict: null });
    };

    const handleAddOpen = () => {
        setAddDialog({ open: true, federalDistrict: { name: "" } });
    };

    const handleAddSave = async () => {
        try {
            const federalDistrict = await createFederalDistrict(
                addDialog.federalDistrict.name
            );
            federalDistricts.push(federalDistrict);
            setFederalDistricts(
                federalDistricts.map((f) => (f.id === addDialog.federalDistrict.id ? addDialog.federalDistrict : f))
            );
            setAddDialog({ open: false, federalDistrict: { name: "" } });
        } catch (err: any) {
            setError(err.response?.data);
        }
    };

    const handleFilterChange = (field: string, value: any) => {
        switch (field) {
            case "name":
                setNameFilter(value);
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
                        {t("addFederalDistrict")}
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
                </div>
            </div>
            <div style={{ margin: "10px" }}>
                <strong>{t("totalRecords")}: {federalDistricts.length}</strong>
            </div>
            <h2>{t("FederalDistrictsTable")}</h2>

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
                                {t("federalDistrictName")}
                            </TableSortLabel>
                        </TableCell>
                        <TableCell>{t("actions")}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {federalDistricts.map((federalDistrict) => (
                        <TableRow
                            key={federalDistrict.id}
                            style={{
                                backgroundColor:
                                federalDistrict.id % 2 === 0 ? "#f9f9f9" : "#ffffff",
                                cursor: "pointer",
                                backgroundColor:
                                    hoveredRow === federalDistrict.id
                                        ? "#d3e0e9"
                                        : federalDistrict.id % 2 === 0
                                            ? "#f9f9f9"
                                            : "#ffffff",
                            }}
                            hover
                            onMouseEnter={() => setHoveredRow(federalDistrict.id)}
                            onMouseLeave={() => setHoveredRow(null)}
                        >
                            <TableCell style={{ textAlign: "center" }}>
                                {federalDistrict.id}
                            </TableCell>
                            <TableCell>{federalDistrict.name}</TableCell>
                            <TableCell>
                                <IconButton onClick={() => handleDeleteConfirm(federalDistrict.id)}>
                                    <DeleteIcon color="error" />
                                </IconButton>
                                <IconButton onClick={() => handleEditOpen(federalDistrict)}>
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

            <Dialog open={addDialog.open} onClose={() => setAddDialog({ open: false, federalDistrict: { name: ""} })}>
                <DialogTitle>{t("addFederalDistrict")}</DialogTitle>
                <DialogContent style={{ padding: "20px" }}>
                    <TextField
                        label={t("name")}
                        fullWidth
                        value={addDialog.federalDistrict.name}
                        onChange={(e) =>
                            setAddDialog({
                                ...addDialog,
                                federalDistrict: { ...addDialog.federalDistrict, name: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={() =>
                            setAddDialog({
                                open: false,
                                federalDistrict: { name: "" },
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

            <Dialog open={editDialog.open} onClose={() => setEditDialog({ open: false, federalDistrict: null })}>
                <DialogTitle>{t("editFederalDistrict")}</DialogTitle>
                <DialogContent style={{ padding: "20px" }}>
                    <TextField
                        label={t("name")}
                        fullWidth
                        value={editDialog.federalDistrict?.name || ""}
                        onChange={(e) =>
                            setEditDialog({
                                ...editDialog,
                                federalDistrict: { ...editDialog.federalDistrict, id: editDialog.federalDistrict.id, name: e.target.value },
                            })
                        }
                        sx={{ marginBottom: 2 }
                        }
                    />
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => setEditDialog({ open: false, federalDistrict: null })}>
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

export default FederalDistrictPage;