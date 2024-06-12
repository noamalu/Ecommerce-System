import React from "react";

interface ProfileStoreInfoProps {
    storeName: string | undefined;
    storeId: number | undefined;
    storeActive: boolean | undefined;
    storeEmailAdd: string | undefined;
    storePhoneNum: string | undefined;
    storeRaiting: number | undefined;
}

const ProfileStoreInfo: React.FC<ProfileStoreInfoProps> = ({ storeName, storeId, storeActive, storeEmailAdd, storePhoneNum, storeRaiting }) => {
  // Check if any of the props are null or undefined
  if (!storeName || !storeId || storeActive === undefined || !storeEmailAdd || !storePhoneNum || storeRaiting === undefined) {
    return (
      <div className="store-details">
        <h2>Store Details</h2>
        <p>Some attributes are missing</p>
      </div>
    );
  }

  return (
    <div className="store-details">
      <h2>Store Details</h2>
      <table>
        <tbody>
          <tr>
            <td>Name:</td>
            <td>{storeName}</td>
          </tr>
          <tr>
            <td>Store ID:</td>
            <td>{storeId}</td>
          </tr>
          <tr>
            <td>Active:</td>
            <td>{storeActive ? 'Yes' : 'No'}</td>
          </tr>
          <tr>
            <td>Email:</td>
            <td>{storeEmailAdd}</td>
          </tr>
          <tr>
            <td>Phone Number:</td>
            <td>{storePhoneNum}</td>
          </tr>
          <tr>
            <td>Rating:</td>
            <td>{storeRaiting}</td>
          </tr>
        </tbody>
      </table>
      {/* Display other details */}
    </div>
  );
};

export default ProfileStoreInfo;
