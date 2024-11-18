import { BottomNavigation, BottomNavigationAction } from "@mui/material";
import AddLocationAltIcon from "@mui/icons-material/AddLocationAlt";
import HomeIcon from "@mui/icons-material/Home";
import MyLocationIcon from "@mui/icons-material/MyLocation";
import { useNavigate } from "react-router-dom";

export default function Footer() {
  const navigate = useNavigate();

  return (
    <footer className="border-t border-gray-300 p-2">
      <BottomNavigation showLabels>
        <BottomNavigationAction
          label="Go home"
          icon={<HomeIcon />}
          onClick={() => navigate("/")}
        />
        <BottomNavigationAction
          label="Active mission"
          onClick={() => navigate("/mission/active")}
          icon={<MyLocationIcon />}
        />
        <BottomNavigationAction
          label="New mission"
          onClick={() => navigate("/mission/create")}
          icon={<AddLocationAltIcon />}
        />
      </BottomNavigation>
    </footer>
  );
}
