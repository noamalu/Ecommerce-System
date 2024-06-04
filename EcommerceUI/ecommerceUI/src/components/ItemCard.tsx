import React, {Component} from "react";
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import placeholder from '../assets/placeholder.jpg';

interface ItemCard {
    itemName: string;
    description: string;
    price: string;
  }

export const ItemCard: React.FC<ItemCard> = ({itemName, description, price}) => {
        return (
            // <h1> card</h1>
            <Card style={{ width: '18rem' }}>
            <Card.Img variant="top" src={placeholder} />
            <Card.Body>
                <Card.Title>{itemName}</Card.Title>
                <Card.Text>
                {description}
                </Card.Text>
                <Card.Text>
                    {price}
                </Card.Text>
                <Button variant="primary" onClick={addToCart}>Add to cart</Button>
            </Card.Body>
            </Card>
        )
}

//make an add to cart request or something
const addToCart = () => {
    // console.log('Clicked on add to cart!');
    alert("clicked on add to cart");
}

export default ItemCard;