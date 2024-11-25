from flask import Flask, jsonify, request
from flask import Flask, request, jsonify
from pymongo import MongoClient
import os
from bson.objectid import ObjectId
from consul import Consul
import uuid
import atexit
# Create a Flask app instance
app = Flask(__name__)
def register_service():
    """Register the service with Consul."""
    #host='consul', port=8500
    consul = Consul(host='consul', port=8500)

    # Get the port from environment variables
    port_str = '5000' # Default to 5000 if not set
    try:
        port = int(port_str)
    except ValueError:
        print(f"Invalid port number: {port_str}")
        return

    # Service registration details
    service_id = f"HealthcarePatientAPI-{uuid.uuid4()}"
    service_name = "PATIENT_SERVICE_NAME"  # Replace with actual service name
    service_address = "healthcarepatientapi"  # Replace with actual address if needed


    consul.agent.service.register(
            name=service_name,
            service_id=service_id,
            address=service_address,
            port=5000
        )

    

    # Ensure to deregister the service on shutdown
    def deregister_service():
        print(f"Deregistering service with ID: {service_id}")
        consul.agent.service.deregister(service_id)

    atexit.register(deregister_service)

# Connect to MongoDB
client = MongoClient('mongodb://mongo:27017/')
db = client['HealthcarePatient']
collection = db['patients']
@app.route('/api/records', methods=['GET'])
def get_persons():
    """Fetch all persons from the database."""
    persons = collection.find()
    results = []
    for person in persons:
        person['Id'] = str(person.pop('_id')) # Convert ObjectId to string
        results.append(person)
    return jsonify(results)

@app.route('/api/records', methods=['POST'])
def add_person():
    """Add a new person to the database."""
    data = request.json
    person_id = collection.insert_one(data).inserted_id
    return jsonify({"message": "Person added", "id": str(person_id)}), 201

@app.route('/api/records', methods=['PUT'])
def update_person():
    """Update a person in the database."""
    data = request.json
    person_id = data.get('Id')
    
    if not person_id:
        return jsonify({"message": "No Id provided"}), 400

    # Ensure person_id is a valid ObjectId
    try:
        person_id = ObjectId(person_id)
    except Exception as e:
        return jsonify({"message": "Invalid Id format"}), 400
    result = collection.update_one({'_id': ObjectId(person_id)}, {'$set': data})
    if result.matched_count:
        return jsonify({"message": "Person updated"})
    else:
        return jsonify({"message": "Person not found"}), 404

@app.route('/api/records/<person_id>', methods=['DELETE'])
def delete_person(person_id):
    """Delete a person from the database."""
    
    result = collection.delete_one({'_id': ObjectId(person_id)})
    if result.deleted_count:
        return jsonify({"message": "Person deleted"})
    else:
        return jsonify({"message": "Person not found"}), 404
 
# Run the app
if __name__ == '__main__':
    register_service()
    app.run(host='0.0.0.0', port=5000)

