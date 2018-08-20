/* 
Book Class 

Controls loading, turning, saving location, and removing pages from the scene.
*/

using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class Book : MonoBehaviour
{

    public GameObject Parent; // Parent of pages
    public GameObject BookInterface; // Book interface

    private string _bookChoices; // Allow you to choose which book to load and view
    private int _currentPage = 0; // Active page in scene
    private int _progress; // Save current page location
    private string _url = "http://cdf7.web.rice.edu/OpenStaxVR_iOS2/"; // Download location for asset bundles

    [NonSerialized]
    public Boolean IsTable = false;

    /* Return the current number of pages avaliable to view. */
    private int TotalPageCount()
    {
        int totalPage;

        if(_bookChoices == "soci")
        {
            totalPage = 19;
        }
        else if(_bookChoices == "bio")
        {
            totalPage = 5;
        }
        else
        {
            totalPage = 4;
        }
        

        return totalPage;
    }
    

    /* Load or download assetbundle and place assets from bundle into scene. */
    IEnumerator DownloadCacheInstantiate (int sibIndex, int pageNumber)
    {
        // Wait for the Caching system to be ready
        while (!Caching.ready)
            yield return null;

        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
        using(WWW www = WWW.LoadFromCacheOrDownload(_url + _bookChoices + "-" + pageNumber, pageNumber)){
            yield return www;
            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);

            // Get desired gameobject
            AssetBundle bundle = www.assetBundle;
            Debug.Log(bundle.Equals(null));
            string str = pageNumber.ToString();
            GameObject loaded = bundle.LoadAsset<GameObject>(str);

            // Place gameobject in scene
            GameObject page = Instantiate(loaded, BookInterface.transform.position, BookInterface.transform.rotation);
            page.transform.parent = Parent.transform;
            page.transform.Rotate(0, 180, 0);
            
            if (pageNumber == _currentPage)
            {
                page.SetActive(true);
            }
            else
            {
                page.SetActive(false);
                
            }

            // Update sibling index
            if (sibIndex != -1)
            {
                page.transform.SetSiblingIndex(sibIndex);
            }

            // Unload bundle
            bundle.Unload(false);
        }
    }   

    /* Update the active page (visible page). */
    public void UpdatePages()
    {
        // Makes any page other than current invisible and makes current visible
        foreach (Transform child in transform)
        {
            // Get page number from page in scene
            string childName = child.name;
            string num = childName.Split('(')[0];
            int index;
            Int32.TryParse(num, out index);

            // Make current page active, others inactive
            if (index == _currentPage)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    /* Update current page to one forward. */
    private int NextPage(int currentPage) 
    {
        if (currentPage == TotalPageCount() - 1) 
        {
            return 0;
        } 
        else 
        {
            currentPage++;
            return currentPage;
        }
    }

    /* Update current page to one back. */
    private int PreviousPage(int currentPage) 
    {
        if (currentPage == 0) 
        {
            return TotalPageCount() - 1;
        } 
        else 
        {
            currentPage--;
            return currentPage;
        }
    }

    /* Go to next page. */
    public void TurnNextPage()
    {
        // Save progress
        _currentPage = NextPage(_currentPage);
        SaveProgress();

        // Remove previous page from scene
        GameObject toDestroy = transform.GetChild(0).gameObject;
        Destroy(toDestroy);

        // Add next page into scene 
        StartCoroutine(DownloadCacheInstantiate(-1, NextPage(_currentPage)));//

        // Update active page
        UpdatePages();
    }

    /* Go back a page. */
    public void TurnBackPage()
    {
        // Save progress
        _currentPage = PreviousPage(_currentPage);
        SaveProgress();

        // Remove next page from scene
        GameObject toDestroy = transform.GetChild(2).gameObject;
        Destroy(toDestroy);

        // Add previous page into scene
        StartCoroutine(DownloadCacheInstantiate(0, PreviousPage(_currentPage)));

        // Update active page
        UpdatePages();
    }

    /* Go to specified page. */
    public void GoToPage(int pageNumber)
    {
        // Save progress
        _currentPage = pageNumber;
        SaveProgress();

        // Destroy any current pages
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Add all required pages to scene
        
        StartCoroutine(DownloadCacheInstantiate(0, PreviousPage(_currentPage)));
        StartCoroutine(DownloadCacheInstantiate(1, _currentPage));
        StartCoroutine(DownloadCacheInstantiate(2, NextPage(_currentPage)));
        
        // Update active page
        UpdatePages();
    }

    /* Save page progress. */
    public void SaveProgress()
    {
        _progress = _currentPage;
        string currentDir = Application.persistentDataPath;        
        string fileName = _bookChoices + "envStatus.txt";
        string fullPath = currentDir + "/" + fileName;
        string bookStatus = "Progress:" + _progress;
        try
        {
            File.WriteAllText(fullPath, bookStatus);
 
        }
        catch (Exception e)
        {
            // Extract some information from this exception, and then   
            // throw it to the parent method.  
            if (e.Source != null)  
                Console.WriteLine("Exception source: {0}", e.Source);  
            throw;  
        }
    }
    
    /* Load page progress. */
    private void LoadProgress()
    {
        // The path of the txt file storing game status.
        string currentDir = Application.persistentDataPath;
        string fileName = _bookChoices + "envStatus.txt";
        string fullPath = currentDir + "/" + fileName;
        try
        {
            int bookStatus;
            // Try reading the progress.
            if (!Int32.TryParse(File.ReadAllText(fullPath).Split(':')[1], out bookStatus))
            {
                // Reset everything.
                _progress = 0;
                _currentPage = 0;
            }
            else
            {
                // Reload game.
                _progress = bookStatus;
                _currentPage = _progress;
            }
        }
        catch (Exception e)
        {
            // Create an empty file.
            File.Create(fullPath).Dispose();
            _progress = 0;
            _currentPage = 0;
        }
    }

    /* Check for saved progress and display required pages on startup. */
    void Start()
    {
        // Load required data
        String bookNameDir = Application.persistentDataPath + "/" + "bookName.txt";
        _bookChoices = File.ReadAllText(bookNameDir);
        LoadProgress();

        // Add required pages into scene
        GoToPage(_currentPage);
    }
}