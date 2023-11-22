import os
import cairosvg
from PIL import Image

def render_svg_to_png(input_svg_path, output_png_path, background_color=(255, 255, 255), output_size=(512, 512)):
    # Render SVG to PNG
    cairosvg.svg2png(url=input_svg_path, write_to=output_png_path, output_height=output_size[0], output_width=output_size[1])

    # Open the PNG image using Pillow
    with Image.open(output_png_path) as img:
        # Create a new image with the desired background color
        new_img = Image.new('RGBA', (img.size[0], img.size[1] + 50), background_color)

        for x in range(output_size[0]):
            for y in range(output_size[1] + 50):
                if(y < 25 or y >= (output_size[1] + 25)):
                    new_img.putpixel((x, y), background_color + (255,))
                else:
                    r, g, b, a = img.getpixel((x, y - 25))
                    # If the pixel has any transparency, set the background color
                    if a != 255:
                        new_img.putpixel((x, y), background_color + (255,))
                    else:
                        new_img.putpixel((x, y), img.getpixel((x, y - 25)))

        # Save the new image with the specified background color
        new_img.save(output_png_path)

def convert_svgs_to_pngs(input_folder, output_folder, background_color=(255, 255, 255), output_size=(512, 512)):
    # Create the output folder if it doesn't exist
    os.makedirs(output_folder, exist_ok=True)

    # Process each SVG file in the input folder
    for filename in os.listdir(input_folder):
        if filename.lower().endswith('.svg'):
            svg_file_path = os.path.join(input_folder, filename)
            png_file_path = os.path.join(output_folder, os.path.splitext(filename)[0] + '.png')

            # Render SVG to PNG with the specified background color and size
            render_svg_to_png(svg_file_path, png_file_path, background_color, output_size)
            print(f'{filename} converted to PNG.')

if __name__ == '__main__':
    input_folder = 'svg_emojis'
    output_folder = 'png_emojis'
    background_color = (255, 204, 77)  # Example: Red color (RGB format)

    convert_svgs_to_pngs(input_folder, output_folder, background_color)
