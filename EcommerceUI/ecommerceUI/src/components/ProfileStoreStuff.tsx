import React from 'react';
import { Table, Dropdown, Form } from 'react-bootstrap';
import { Role } from './ProfileStoreNav';


interface TableRowProps {
    role: Role;
}

const TableRow: React.FC<TableRowProps> = ({ role }) => {
    return (
        <tr>
            <td>{role.username}</td>
            <td>{role.appointer}</td>
            <td>{role.appointer}</td>
            <td>
                <Dropdown>
                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                        Permissions
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item href="#/action-1">
                            <Form>
                                {role.permissions.map((permission, index) => (
                                    <div key={`permission-${index}`} className="mb-3">
                                        <Form.Check
                                            type="checkbox"
                                            id={`permission-checkbox-${index}`}
                                            label={permission}
                                        />
                                    </div>
                                ))}
                            </Form>
                        </Dropdown.Item>
                    </Dropdown.Menu>
                </Dropdown>
            </td>
        </tr>
    );
};

interface MyTableProps {
    roles: Role[];
}

const MyTable: React.FC<MyTableProps> = ({ roles }) => {
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
                {roles.map((role, index) => (
                    <TableRow key={`role-${index}`} role={role} />
                ))}
            </tbody>
        </Table>
    );
};

export default MyTable;
