import React, {Component, useState} from "react";
import { Container } from "react-bootstrap";

import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import Nav from 'react-bootstrap/Nav';


export const Profile = () => {
    const [num] = useState(0);
    return (
        <>
            <Row className="full-height-row">
                <Col md={2} className="profile-left">
                    <Stack gap={2} >
                        <Button variant="outline-secondary">Profile option 1</Button>
                        <Button variant="outline-secondary">Profile option 2</Button>
                        <Button variant="outline-secondary">Profile option 3</Button>
                        <Button variant="outline-secondary">Profile option 4</Button>
                    </Stack>
                </Col>
                <Col  className="profile-right">
                    <Nav variant="tabs" defaultActiveKey="/home">
                    <Nav.Item>
                        <Nav.Link href="">Store 1</Nav.Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Nav.Link eventKey="">Store 2</Nav.Link>
                    </Nav.Item>
                    <Nav.Item>
                        <Nav.Link eventKey="disabled" disabled>
                        Disabled
                        </Nav.Link>
                    </Nav.Item>
                    </Nav>
                    
                </Col>
            </Row>
        
        </>
      );
}