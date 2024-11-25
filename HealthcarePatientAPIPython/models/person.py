from pydantic import BaseModel, Field, EmailStr
from typing import Optional
from datetime import datetime
from utils.location import Location  # Import Location model

# Person model
class Person(BaseModel):
    first_name: str
    last_name: str
    contact_name: Optional[str]
    mobile_phone: Optional[str]
    fax: Optional[str]
    email: Optional[EmailStr]
    location: Optional[Location]  # Updated to use the Location model
    is_active: bool = True
    date_of_birth: datetime
