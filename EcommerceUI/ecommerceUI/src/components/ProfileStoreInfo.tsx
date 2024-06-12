import React from "react";

interface StoreDetailsProps {
  storeDetails: {
    name: string;
    phoneNumber: string;
    email: string;
    rating: number;
    // Add other details as needed
  } | null;
}

const StoreDetails: React.FC<StoreDetailsProps> = ({ storeDetails }) => {
  if (!storeDetails) {
    return <p>Loading...</p>;
  }

  const { name, phoneNumber, email, rating } = storeDetails;

  return (
    <div className="store-details">
      <h2>Store Details</h2>
      <p>Name: {name}</p>
      <p>Phone Number: {phoneNumber}</p>
      <p>Email: {email}</p>
      <p>Rating: {rating}</p>
      {/* Display other details */}
    </div>
  );
};

export default StoreDetails;
