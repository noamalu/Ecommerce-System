import React, { useState } from 'react';
import {BrowserRouter, Routes, Route} from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { Home } from "./pages/Home"
import { Login } from "./pages/Login"
import { NavBar } from "./components/NavBar"
import { Register } from './pages/Register';

function App() {
  return (
    <div>
      <NavBar />
      <BrowserRouter>
        <Routes>
          <Route index element = {<Home/>}/>
          <Route path="/home" element={<Home/>}/>
          <Route path="/login" element={<Login/>}/>
          <Route path="/Register" element={<Register/>}/>
        </Routes>
      </BrowserRouter>
       
    </div>
  );
}

export default App;
