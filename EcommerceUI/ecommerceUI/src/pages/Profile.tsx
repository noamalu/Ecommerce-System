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



export const Profile = () => {
    const [num, setNum] = useState(1);

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
                    <Nav variant="tabs" defaultActiveKey="/home">
                        <Nav.Item>
                            <Nav.Link href="" onClick={() => setNum(1)}>Store 1</Nav.Link>
                        </Nav.Item>
                        <Nav.Item>
                            <Nav.Link onClick={() => setNum(2)}>Store 2</Nav.Link>
                        </Nav.Item>
                    </Nav>
                    <p> store {num} </p>
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                            <th>#</th>
                            <th>name</th>
                            <th>phone number</th>
                            <th>role</th>
                            <th>appointer</th>
                            <th>permissions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                            <td>1</td>
                            <td>Mark</td>
                            <td>+972 52-123-4567</td>
                            <td>Founder</td>
                            <td></td>
                            <td> 
                                <Dropdown>
                                <Dropdown.Toggle variant="success" id="dropdown-basic">
                                    Dropdown Button
                                </Dropdown.Toggle>

                                <Dropdown.Menu>
                                    <Dropdown.Item href="#/action-1">
                                        <Form>
                                                <div key="default-checkbox" className="mb-3">
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="permission 1"
                                                />
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="permission 2"
                                                />
                                                <Form.Check
                                                    type="checkbox"
                                                    id="default-checkbox"
                                                    label="permission 3"
                                                />
                                                </div>
                                        </Form>
                                    </Dropdown.Item>
                                </Dropdown.Menu>
                                </Dropdown>
                            </td>
                            </tr>
                        </tbody>
                        </Table>
                </Col>
            </Row>
        
        </>
      );
}