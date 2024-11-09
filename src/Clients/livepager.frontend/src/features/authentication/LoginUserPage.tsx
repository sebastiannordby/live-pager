// src/features/auth/LoginPage.jsx
import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";
import { API } from "../../data/api";
import { Button, TextField } from "@mui/material";

export default function LoginUserPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e: FormEvent) => {
    e.preventDefault();

    try {
      const response = await API.authentication.login({
        username,
        password,
      });

      if (response.jwtToken) {
        localStorage.setItem("token", response.jwtToken);
        navigate("/tracking");
      } else {
        setError("Invalid username or password");
      }
    } catch (err) {
      setError("Login failed. Try again.");
    }
  };

  return (
    <div className="flex flex-col items-center justify-center">
      <h2 className="text-lg mb-1">Login</h2>
      {error && <p className="text-red-500">{error}</p>}
      <form onSubmit={handleLogin} className="flex flex-col space-y-4">
        <TextField
          label="Username"
          variant="outlined"
          autoComplete="off"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          fullWidth
          margin="normal"
        />
        <TextField
          label="Password"
          type="password"
          variant="outlined"
          autoComplete="off"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          margin="normal"
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          sx={{ mt: 2 }}
        >
          Login
        </Button>
        <Button
          type="button"
          variant="contained"
          color="secondary"
          fullWidth
          href="/authentication/register"
          sx={{ mt: 0 }}
        >
          Dont have a user? Register here
        </Button>
      </form>
    </div>
  );
}
