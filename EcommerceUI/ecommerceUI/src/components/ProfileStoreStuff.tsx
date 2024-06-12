import React from 'react';
import { Table, Dropdown, Form } from 'react-bootstrap';

const TableRow = () => {
    return (
        <tr>
            <td>1</td>
            <td>Mark</td>
            <td>+972 52-123-4567</td>
            <td>Founder</td>
            <td></td>
            <td>
                <Dropdown>
                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                        Permissions
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item href="#/action-1">
                            <Form>
                                <div key="default-checkbox" className="mb-3">
                                    <Form.Check
                                        type="checkbox"
                                        id="default-checkbox-1"
                                        label="Permission 1"
                                    />
                                    <Form.Check
                                        type="checkbox"
                                        id="default-checkbox-2"
                                        label="Permission 2"
                                    />
                                    <Form.Check
                                        type="checkbox"
                                        id="default-checkbox-3"
                                        label="Permission 3"
                                    />
                                </div>
                            </Form>
                        </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </td>
        </tr>
    );
};

const MyTable = () => {
    return (
        <Table striped bordered hover className="my-3">
            <thead>
                <tr>
                    <th>#</th>
                    <th>Name</th>
                    <th>Phone Number</th>
                    <th>Role</th>
                    <th>Appointer</th>
                    <th>Permissions</th>
                </tr>
            </thead>
            <tbody>
                <TableRow />
            </tbody>
        </Table>
    );
};

export default MyTable;
