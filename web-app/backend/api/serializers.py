from rest_framework import serializers
from .models import Image

# Serializers translate data from models to formats like
# JSON that are useful for APIs.

class ImageSerializer(serializers.ModelSerializer):
    class Meta:
        model = Image
        fields = ['id', 'encoded_image']