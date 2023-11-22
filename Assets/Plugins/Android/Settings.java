package com.micahhenney.wallpaper;

import static android.Manifest.permission.READ_EXTERNAL_STORAGE;

import android.annotation.TargetApi;
import android.app.Activity;
import android.app.WallpaperManager;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.service.wallpaper.WallpaperService;
import android.util.Log;
import android.view.View;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;

public class Settings extends Activity {
    public static final int PICK_IMAGE = 1;


    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.settings);

        findViewById(R.id.imageButton).setOnClickListener(view -> {
            getImage();
        });
        ((CheckBox) findViewById(R.id.useEmojis)).setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                // Display a toast with the CheckBox state
                SharedPreferences settings = getSharedPreferences("com.micahhenney.wallpaper.v2.playerprefs", Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = settings.edit();
                editor.putBoolean("useEmojis", isChecked);
                editor.commit();
            }
        });
        findViewById(R.id.restartButton).setOnClickListener(view -> {
            restartWallpaper();
        });
    }

    @TargetApi(Build.VERSION_CODES.M)
    private void getImage() {
        Intent getIntent = new Intent(Intent.ACTION_GET_CONTENT);
        getIntent.setType("image/*");

        Intent pickIntent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
        pickIntent.setType("image/*");

        Intent chooserIntent = Intent.createChooser(getIntent, "Select Image");
        chooserIntent.putExtra(Intent.EXTRA_INITIAL_INTENTS, new Intent[] {pickIntent});
        if (checkSelfPermission(READ_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED) {
            startActivityForResult(chooserIntent, PICK_IMAGE);
        } else {
            requestPermissions(new String[]{READ_EXTERNAL_STORAGE}, 0);
        }
    }

    @Override
    public void onActivityResult(int reqCode, int resCode, Intent data) {
        if(reqCode == PICK_IMAGE) {
            // Save the image data to the persistent data path
            if(resCode == RESULT_OK) {
                // Get the image data
                Uri selectedImage = data.getData();
                String[] filePathColumn = {MediaStore.Images.Media.DATA};
                Cursor cursor = getContentResolver().query(selectedImage, filePathColumn, null, null, null);
                cursor.moveToFirst();
                int columnIndex = cursor.getColumnIndex(filePathColumn[0]);
                String filePath = cursor.getString(columnIndex);
                cursor.close();

                // Save the image data to the persistent data path
                SharedPreferences settings = getSharedPreferences("com.micahhenney.wallpaper.v2.playerprefs", Context.MODE_PRIVATE);
                SharedPreferences.Editor editor = settings.edit();
                editor.putString("backImageLoc", filePath);
                editor.commit();

                // Copy the image to the app persistent data
                try {
                    FileInputStream in = new FileInputStream(filePath);
                    File outFile = new File(getExternalFilesDir(null), "backImage.jpg");
                    FileOutputStream out = new FileOutputStream(outFile);
                    byte[] buffer = new byte[1024];
                    int read;
                    while ((read = in.read(buffer)) != -1) {
                        out.write(buffer, 0, read);
                    }
                    in.close();
                    out.close();
                } catch (Exception e) {
                    Log.e("Error", e.getMessage());
                    e.printStackTrace();
                }
            }

            // Restart the wallpaper service
            // restartWallpaper();
        }
    }
    private void restartWallpaper() {
        WallpaperManager m = WallpaperManager.getInstance(this);
        try {
            m.clear();
        } catch (IOException e) {
            e.printStackTrace();
        }
        Intent intent = new Intent(
                WallpaperManager.ACTION_CHANGE_LIVE_WALLPAPER);
        intent.putExtra(WallpaperManager.EXTRA_LIVE_WALLPAPER_COMPONENT,
                new ComponentName(this, SimpleWallpaperService.class));
        startActivity(intent);
    }
}