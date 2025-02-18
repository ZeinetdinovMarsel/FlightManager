// src/components/UserProfile.tsx
import React, { useEffect, useState } from "react";
import { useSettingsStore } from "../store/settings";
import {
  Container,
  Typography,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  Grid,
  Box,
} from "@mui/material";

const UserProfile: React.FC = () => {
  const settings = useSettingsStore();
  const [language, setLanguage] = useState(settings.language);
  const [rowsPerPage, setRowsPerPage] = useState<number>(settings.rowsPerPage);
  const [updateTime, setUpdateTime] = useState<number>(settings.updateTime);

  useEffect(() => {
    setLanguage(settings.language);
    setRowsPerPage(settings.rowsPerPage);
    setUpdateTime(settings.updateTime);
  }, [settings]);

  const handleSave = () => {
    settings.setLanguage(language);
    settings.setRowsPerPage(rowsPerPage);
    settings.setUpdateTime(updateTime);
  };

  return (
    <Container component="main" maxWidth="xs">
      <Box sx={{ marginTop: 8, display: "flex", flexDirection: "column", alignItems: "center" }}>
        <Typography component="h1" variant="h5">
          Настройки профиля
        </Typography>
        <Grid container spacing={2} sx={{ marginTop: 2 }}>
          <Grid item xs={12}>
            <FormControl fullWidth>
              <InputLabel id="language-label">Язык</InputLabel>
              <Select
                labelId="language-label"
                id="language-select"
                value={language}
                label="Язык"
                onChange={(e) => setLanguage(e.target.value as "ru" | "en")}
              >
                <MenuItem value="ru">Русский</MenuItem>
                <MenuItem value="en">English</MenuItem>
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12}>
            <TextField
              label="Записей на странице"
              type="number"
              fullWidth
              value={rowsPerPage}
              onChange={(e) => setRowsPerPage(parseInt(e.target.value, 10))}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              label="Время обновления в секундах"
              type="number"
              fullWidth
              value={updateTime}
              onChange={(e) => setUpdateTime(parseInt(e.target.value, 10))}
            />
          </Grid>
          <Grid item xs={12}>
            <Button variant="contained" color="primary" onClick={handleSave}>
              Сохранить
            </Button>
          </Grid>
        </Grid>
      </Box>
    </Container>
  );
};

export default UserProfile;