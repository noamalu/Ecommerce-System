import React, {Component, useState} from "react";
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import  Container  from "react-bootstrap/Container";
import { useLocation } from 'react-router-dom';


// interface Search{
//     query: string;
// }




export const Search = () => {
    console.log("Search");
    const location = useLocation();
    const query = new URLSearchParams(location.search).get('query');
    alert(query);
    // call api for current query with no filters
    

    return(
        <Container>
            <Row className="full-height-row">
                <Col md={2} className="profile-left">
                    <Stack gap={2} >
                        <p> filter by </p>
                        <Button variant="outline-secondary">Rank</Button>
                        <Button variant="outline-secondary">Price</Button>
                        <Button variant="outline-secondary">category</Button>
                        <Button variant="outline-secondary">apply filters</Button>
                    </Stack>
                </Col>
                <Col  className="profile-right">
                    //show results according to results
                </Col>
                </Row>
        </Container>
    );

}