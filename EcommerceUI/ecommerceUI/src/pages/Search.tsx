import React, {Component, useState} from "react";
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import  Container  from "react-bootstrap/Container";
import { useLocation } from 'react-router-dom';



export const Search = () => {
    console.log("Search");
    const location = useLocation();
    const query = new URLSearchParams(location.search).get('query');
    const filter = new URLSearchParams(location.search).get('filter');
    // call api for current query with filter
    
    return(
        <>
            <p> query = {query} filter = {filter}</p>
        </>
    );

}