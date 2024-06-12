import React from 'react';
import { Table, Dropdown, Form } from 'react-bootstrap';
import { Role } from './ProfileStoreNav';

interface TableRowProps {
    role: Role;
    index: number;
}

const TableRow: React.FC<TableRowProps> = ({ role, index }) => {
    return (
        <tr>
            <td>{index + 1}</td> {/* Add index + 1 here */}
            <td>{role.username}</td>
            <td>{role.role}</td>
            <td>{role.appointer}</td>
            <td>
                <Dropdown>
                    <Dropdown.Toggle variant="success" id="dropdown-basic">
                        Permissions
                    </Dropdown.Toggle>
                    <Dropdown.Menu>
                        <Dropdown.Item>
                            <Form>
                                {role.permissions && role.permissions.length > 0 ? (
                                    role.permissions.map((permission, index) => (
                                        <div key={`permission-${index}`} className="mb-3">
                                            <Form.Check
                                                type="checkbox"
                                                id={`permission-checkbox-${index}`}
                                                label={permission}
                                            />
                                        </div>
                                    ))
                                ) : (
                                    <div className="mb-3">No Permissions</div>
                                )}
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
                    <th>Role</th>
                    <th>Appointer</th>
                    <th>Permissions</th>
                </tr>
            </thead>
            <tbody>
                {roles.map((role, index) => (
                    <TableRow key={`role-${index}`} role={role} index={index} />
                ))}
            </tbody>
        </Table>
    );
};

export default MyTable;
