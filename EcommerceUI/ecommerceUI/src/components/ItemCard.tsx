import React, {Component} from "react";
import Button from 'react-bootstrap/Button';
import Card from 'react-bootstrap/Card';
import placeholder from '../assets/placeholder.jpg';
import { getToken } from "../services/SessionService";

interface ItemCard {
    itemName: string;
    description: string;
    price: string;
    addToCart: (storeId: number, productId: number) => void;
    storeId: number;
    productId: number;
}

export const ItemCard: React.FC<ItemCard> = ({storeId, productId,itemName, description, price}) => {
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
                <Button variant="primary" onClick={() => addToCart(storeId, productId)}>Add to cart</Button>
            </Card.Body>
            </Card>
        )
}

const addToCart =  (storeid: number, productId: number) => { 
  fetch(`https://localhost:7163/api/Client/Cart?identifier=${getToken()}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
                storeId: storeid,
                id: productId,
                quantity: 1,
              })
  }).then((r) => {
    if (r.ok) {
      console.log("put in cart worked")
      return;
    } else {
      throw new Error('put in cart didnt work');
    }
  })
  .catch((error) => {
    window.alert(error.message);
  });
}

export default ItemCard;
