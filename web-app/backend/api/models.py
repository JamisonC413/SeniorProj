from django.db import models

# Create your models here.

class Image(models.Model):
    # Primary key 'id' automatically inserted by Django
    encoded_image = models.TextField()

    def __str__(self):
        return "Image " + str(self.id)