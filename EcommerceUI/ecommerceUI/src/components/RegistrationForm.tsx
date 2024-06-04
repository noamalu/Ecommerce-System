// src/RegistrationForm.tsx
import React, { useState } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';


interface FormData {
  username: string;
  email: string;
  password: string;
}

interface Errors {
  username?: string;
  email?: string;
  password?: string;
}

const RegistrationForm: React.FC = () => {
  const [formData, setFormData] = useState<FormData>({
    username: '',
    email: '',
    password: ''
  });

  const [errors, setErrors] = useState<Errors>({});

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const validate = (): Errors => {
    let formErrors: Errors = {};
    if (!formData.username) formErrors.username = 'Username is required';
    if (!formData.email) formErrors.email = 'Email is required';
    else if (!/\S+@\S+\.\S+/.test(formData.email)) formErrors.email = 'Email is invalid';
    if (!formData.password) formErrors.password = 'Password is required';
    else if (formData.password.length < 6) formErrors.password = 'Password must be at least 6 characters';
    return formErrors;
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formErrors = validate();
    if (Object.keys(formErrors).length === 0) {
      console.log('Form data:', formData);
      // Here you can handle form submission (e.g., send data to a server)
      alert("Registered successfully");
    } else {
      setErrors(formErrors);
    }
  };

  return (
    <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3">
          <Form.Label>Username</Form.Label>
          <Form.Control type="text"
            name="username"
            value={formData.username}
            onChange={handleChange} 
            placeholder="username" />
            {errors.username && <p>{errors.username}</p>}
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label>Email</Form.Label>
          <Form.Control type="text"
            name="email"
            value={formData.email}
            onChange={handleChange} 
            placeholder="email" />
            {errors.email && <p>{errors.email}</p>}
        </Form.Group>

        <Form.Group className="mb-3" >
          <Form.Label>Password</Form.Label>
          <Form.Control type="password" placeholder="Password" 
                        name="password"
                        value={formData.password}
                        onChange={handleChange}/>
                        {errors.password && <p>{errors.password}</p>}
        </Form.Group>
        <Button variant="primary" type="submit">
          Register
        </Button>
      </Form>
  );
};

export default RegistrationForm;
