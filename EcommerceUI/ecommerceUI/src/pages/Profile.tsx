import React, {Component, useState} from "react";
import { Container } from "react-bootstrap";
import Image from 'react-bootstrap/Image';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Button from 'react-bootstrap/Button';
import Stack from 'react-bootstrap/Stack';
import Nav from 'react-bootstrap/Nav';
import avatar from '../assets/avatar.jpg';
import Table from 'react-bootstrap/Table';
import Dropdown from 'react-bootstrap/Dropdown';
import Form from 'react-bootstrap/Form';
import InputGroup from 'react-bootstrap/InputGroup';
import { ProfileStoreNav } from "../components/ProfileStoreNav";



export const Profile = () => {
    

    return (
        <>
            <Row className="full-height-row">
                <Col md={2} className="profile-left">
                    <Stack gap={2} >
                        <Image src={avatar} roundedCircle className="w-25 mx-auto" />
                        <p> John Doe</p>
                        <Button variant="outline-secondary">Profile option 1</Button>
                        <Button variant="outline-secondary">Profile option 2</Button>
                        <Button variant="outline-secondary">Profile option 3</Button>
                        <Button variant="outline-secondary">Profile option 4</Button>
                    </Stack>
                </Col>
                
                <Col  className="profile-right">
                    
                    <ProfileStoreNav/>
                    
                </Col>
            </Row>
        
        </>
      );
}