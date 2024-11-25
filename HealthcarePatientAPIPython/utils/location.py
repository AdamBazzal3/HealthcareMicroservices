from pydantic import BaseModel
from typing import Optional

# Location model
class Location(BaseModel):
    address: Optional[str]
    city: Optional[str]
    country: Optional[str]
