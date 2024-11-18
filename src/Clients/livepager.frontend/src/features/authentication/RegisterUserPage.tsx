import { CreateUserRequest } from "../../data/models/authentication";
import React, { useState } from "react";
import { TextField, Button, Box } from "@mui/material";
import { API } from "../../data/api";
import { useNavigate } from "react-router-dom";

const CreateUserForm: React.FC = () => {
  const navigate = useNavigate();
  const [formValues, setFormValues] = useState<CreateUserRequest>({
    username: "",
    email: "",
    displayName: null,
    password: "",
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormValues({
      ...formValues,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const response = await API.authentication.createUser(formValues);
    if (response) {
      navigate("/authentication/login");
    }
  };

  return (
    <div className="p-4">
      <Box
        component="form"
        onSubmit={handleSubmit}
        sx={{ display: "flex", flexDirection: "column", gap: 2 }}
      >
        <TextField
          label="Username"
          name="username"
          autoComplete="off"
          value={formValues.username}
          onChange={handleInputChange}
          required
        />
        <TextField
          label="Email"
          name="email"
          type="email"
          autoComplete="off"
          value={formValues.email}
          onChange={handleInputChange}
          required
        />
        <TextField
          label="Display Name"
          name="displayName"
          autoComplete="off"
          value={formValues.displayName || ""}
          onChange={handleInputChange}
          placeholder="Optional"
        />
        <TextField
          label="Password"
          name="password"
          type="password"
          autoComplete="off"
          value={formValues.password}
          onChange={handleInputChange}
          required
        />
        <Button type="submit" variant="contained" color="primary">
          Register
        </Button>
      </Box>
    </div>
  );
};

export default CreateUserForm;
