import { useParams } from "react-router-dom";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { useTranslation } from "react-i18next";

const columns: GridColDef[] = [
  { field: "id", headerName: "ID", width: 70 },
  { field: "name", headerName: "Name", width: 130 },
  { field: "age", headerName: "Age", width: 90, align: "center" },
];

const rows = [
  { id: 1, name: "John Doe", age: 25 },
  { id: 2, name: "Jane Smith", age: 30 },
];

export default function TablesPage() {
  const { tableName } = useParams();
  const { t } = useTranslation();

  return (
    <div>
      <h2>{t("table")}: {tableName}</h2>
      <DataGrid 
        rows={rows} 
        columns={columns} 
        pageSizeOptions={[5, 10, 20]} 
        pagination 
      />
    </div>
  );
}
