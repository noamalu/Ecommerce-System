import React, {Component} from "react";
import { ItemCard } from '../components/ItemCard';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';

export const Home = () => {
    return (
        <>
            <Row xs={1} md={4} className="g-4">
            {Array.from({ length: 8 }).map((_, idx) => (
                <Col key={idx}>
                <ItemCard itemName="Item Name" 
                            description="This is some long long item description. I don't know, it's from China."
                            price="19.99₪"
                            />
                </Col>
            ))}
            </Row>
        </>
      );
}