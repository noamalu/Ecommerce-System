import React, {Component} from "react";
import { ItemCard } from '../components/ItemCard';
import Col from 'react-bootstrap/Col';
import Row from 'react-bootstrap/Row';
import Container from 'react-bootstrap/Container';


export const Home = () => {
    return (
        <>
            <Container className="flex">
                <Row xs={1} md={4} className="g-4">
                {Array.from({ length: 8 }).map((_, idx) => (
                    <Col key={idx}>
                    <ItemCard itemName="Item Name"
                        description="This is some long long item description. I don't know, it's from China."
                        price="19.99₪" addToCart={function (storeId: number, productId: number): void {
                            throw new Error("Function not implemented.");
                        } } storeId={0} productId={0}                            />
                    </Col>
                ))}
                </Row>
            </Container>
        </>
      );
}