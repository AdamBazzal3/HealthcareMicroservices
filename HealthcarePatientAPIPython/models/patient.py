
from pydantic import Field
from typing import Optional
from bson import ObjectId
from enum import Enum
from person import Person
class Gender(str, Enum):
    female = 'Female'
    male = 'Male'

# Patient model for MongoDB
class Patient(Person):
    gender: Gender
    insurance_details: Optional[str]

    class Config:
        allow_population_by_field_name = True
        json_encoders = {ObjectId: str}
